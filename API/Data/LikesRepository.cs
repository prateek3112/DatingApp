using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class LikesRepository : ILikeRepository
    {
        private readonly DataContext context;
        public LikesRepository(DataContext context)
        {
            this.context = context;
        }

        // To find single like
        public async Task<UserLike> GetUserLike(int SourceUserId, int LikedUserId)
        {
            var userLike = await context.Likes.FindAsync(SourceUserId, LikedUserId);
            return userLike;
        }

        public async Task<PagedList<LikeDto>> GetUserLikes(LikesParams likesParams)
        {
            var users = context.Users.OrderBy(x => x.UserName).AsQueryable();
            var likes = context.Likes.AsQueryable();

            if (likesParams.Predicate == "liked")
            {
                likes = likes.Where(likes => likes.SourceUserId == likesParams.UserId);
                users = likes.Select(like => like.LikedUser);
            }

            if (likesParams.Predicate == "likedBy")
            {
                likes = likes.Where(likes => likes.LikedUserId == likesParams.UserId);
                users = likes.Select(like => like.SourceUser);
            }

            var likesUsers = users.Select(user => new LikeDto
            {
                UserName = user.UserName,
                Name = user.Name,
                Age = user.DateOfBirth.CalculateAge(),
                City = user.City,
                PhotoUrl = user.Photos.FirstOrDefault(p => p.IsMain).Url,
                Id = user.Id
            });
 
            return await PagedList<LikeDto>.CreateList(likesUsers,likesParams.PageNumber,likesParams.PageSize);

        }

        //Returns List of Users this User has liked
        public async Task<AppUser> GetUserWithLikes(int userId)
        {
            return await context.Users.Include(x => x.LikedUsers).FirstOrDefaultAsync(x => x.Id == userId);
        }
    }
}