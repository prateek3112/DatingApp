using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class LikesController : BaseApiController
    {
        private readonly IUserRepository userRepository;
        private readonly ILikeRepository likeRepository;
        public LikesController(IUserRepository userRepository, ILikeRepository likeRepository)
        {
            this.likeRepository = likeRepository;
            this.userRepository = userRepository;
        }


        [HttpPost("{username}")]
        public async Task<ActionResult> AddLike(string username)
        {
            var SourceUserId = User.GetUserId();
            var LikedUser = await userRepository.GetUserByUsernameAsync(username);
            var SourceUser = await likeRepository.GetUserWithLikes(SourceUserId);

            if (LikedUser == null) return NotFound();

            if (SourceUser.UserName == username) return BadRequest("You cannot like yourself!");

            var userLike = await likeRepository.GetUserLike(LikedUser.Id,SourceUserId);

            if (userLike != null) return BadRequest("You have already liked this user!");

            userLike = new UserLike
            {
                SourceUserId = SourceUserId,
                LikedUserId = LikedUser.Id
            };

            SourceUser.LikedUsers.Add(userLike);

            if (await userRepository.SaveAllAsync()) return Ok();

            return BadRequest("Failed to like user :(");
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LikeDto>>> GetUserLikes([FromQuery]LikesParams likesParams)
        {
            likesParams.UserId = User.GetUserId();
            var users = await likeRepository.GetUserLikes(likesParams);

Response.AddPaginationHeader(users.CurrentPage,users.TotalCount,users.PageSize,users.TotalPages);

            return Ok(users);
        }

    }
}