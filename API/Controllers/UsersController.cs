using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Extensions;

namespace API.Controllers
{
    [Authorize]
    public class UsersController : BaseApiController
    {
        private readonly IPhotoService photoService;

        private readonly IUserRepository UserRepository;

        private readonly IMapper Mapper;
        public UsersController(IUserRepository userRepository, IMapper mapper, IPhotoService photoService)
        {
            this.Mapper = mapper;
            this.photoService = photoService;
            this.UserRepository = userRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers([FromQuery] UserParams userParams)
        {
            var user = await this.UserRepository.GetUserByUsernameAsync(User.GetUsername());
            userParams.CurrentUsername = user.UserName;
            if (string.IsNullOrEmpty(user.Gender))
            {
                userParams.Gender = user.Gender == "male" ? "female" : "male";
            }

            var users = await this.UserRepository.GetMembersAsync(userParams);



            Response.AddPaginationHeader(users.CurrentPage, users.TotalCount, users.PageSize, users.TotalPages);

            return Ok(users);

        }


        [HttpGet("{username}", Name = "GetUser")]
        public async Task<ActionResult<MemberDto>> GetUser(string username)
        {
            return await this.UserRepository.GetMemberAsync(username);

        }

        [HttpPut]
        public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
        {

            var user = await UserRepository.GetUserByUsernameAsync(User.GetUsername());

            Mapper.Map(memberUpdateDto, user);

            UserRepository.Update(user);

            if (await UserRepository.SaveAllAsync()) return NoContent();

            return BadRequest("Failed to Update the User :(");

        }

        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoDto>> UploadPhoto(IFormFile file)
        {

            var user = await UserRepository.GetUserByUsernameAsync(User.GetUsername());
            var result = await photoService.AddPhotoAsync(file);
            if (result.Error != null) return BadRequest(result.Error.Message);

            var photo = new Photo
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId

            };

            if (user.Photos.Count == 0)
            {
                photo.IsMain = true;
            }

            user.Photos.Add(photo);

            if (await UserRepository.SaveAllAsync())
            {


                return CreatedAtRoute("GetUser", new { username = user.UserName }, Mapper.Map<PhotoDto>(photo));
            }

            return BadRequest("Problem Uploading Photo");

        }

        [HttpPut("set-main-photo/{photoId}")]
        public async Task<ActionResult> SetMainPhoto(int photoId)
        {
            var user = await UserRepository.GetUserByUsernameAsync(User.GetUsername());

            var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);

            if (photo.IsMain) return BadRequest("This is already your Main photo");

            var currentMain = user.Photos.FirstOrDefault(x => x.IsMain);

            if (currentMain != null) currentMain.IsMain = false;

            photo.IsMain = true;

            if (await UserRepository.SaveAllAsync()) return NoContent();

            return BadRequest("Failed to set as main photo");




        }


        [HttpDelete("delete-photo/{photoId}")]
        public async Task<ActionResult> DeletePhoto(int photoId)
        {
            var user = await UserRepository.GetUserByUsernameAsync(User.GetUsername());

            var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);

            if (photo == null) return NotFound("Not Found");

            if (photo.IsMain) return BadRequest("Cannot Delete Main Photo :(");

            if (photo.PublicId != null)
            {
                var result = await this.photoService.DeletPhotoAsync(photo.PublicId);
                if (result.Error != null) return BadRequest("Something Went Wrong! :(");
            }

            user.Photos.Remove(photo);

            if (await UserRepository.SaveAllAsync()) return Ok();

            else
            {
                return BadRequest("Something Went Wrong! :(");
            }
        }
    }

}