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
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
        {
            return Ok(await this.UserRepository.GetMembersAsync());

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

            if(photo.IsMain) return BadRequest("This is already your Main photo");

            var currentMain = user.Photos.FirstOrDefault(x => x.IsMain);

            if(currentMain != null) currentMain.IsMain = false;

            photo.IsMain = true;

if(await UserRepository.SaveAllAsync()) return NoContent();

return BadRequest("Failed to set as main photo");




        }
    }
}