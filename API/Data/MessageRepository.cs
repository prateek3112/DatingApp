using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class MessageRepository : IMessageRepository
    {
        private readonly DataContext context;
        private readonly IMapper mapper;
        public MessageRepository(DataContext context, IMapper mapper)
        {
            this.mapper = mapper;
            this.context = context;
        }

        public void AddMessage(Message message)
        {
            context.Messages.Add(message);
        }

        public void DeleteMessage(Message message)
        {
            context.Messages.Remove(message);
        }

        public async Task<Message> GetMessage(int MessageId)
        {
            return await context.Messages
            .Include(u => u.Sender)
            .Include(u => u.Recipient)
            .SingleOrDefaultAsync(x=>x.Id == MessageId);
        }

        public async Task<PagedList<MessageDto>> GetMessagesForUser(MessageParams messageParams)
        {
            var query = context.Messages.OrderByDescending(m => m.SentAt).AsQueryable();

            query = messageParams.Container switch
            {
                "Inbox" => query.Where(u => u.Recipient.UserName == messageParams.Username && u.RecipientDeleted == false),
                "Outbox" => query.Where(u => u.Sender.UserName == messageParams.Username && u.SenderDeleted == false),
                _ => query.Where(u => u.Recipient.UserName == messageParams.Username && u.RecipientDeleted == false && u.ReadAt == null)
            };

            var messages = query.ProjectTo<MessageDto>(mapper.ConfigurationProvider);

            return await PagedList<MessageDto>.CreateList(messages,messageParams.PageNumber,messageParams.PageSize);
        }

        public async Task<IEnumerable<MessageDto>> GetMessageThread(string currentUsername, string RecipientUsername)
        {
            var messages = await context.Messages
            .Include(u => u.Sender).ThenInclude(p=>p.Photos)
            .Include(u => u.Recipient).ThenInclude(p=>p.Photos)
            .Where(m => m.Recipient.UserName == currentUsername && m.RecipientDeleted == false && m.Sender.UserName == RecipientUsername || m.Recipient.UserName == RecipientUsername && m.Sender.UserName == currentUsername && m.SenderDeleted == false)
            .OrderBy(m => m.SentAt)
            .ToListAsync();


            var Unread = messages.Where(m =>m.ReadAt == null && m.Recipient.UserName == currentUsername).ToList();

if(Unread.Any())
{
    foreach (var unreadMessage in Unread)
    {
       unreadMessage.ReadAt = System.DateTime.Now;  
    }

    await context.SaveChangesAsync();
}

return  mapper.Map<IEnumerable<MessageDto>>(messages);

        }

        public async Task<bool> SaveAllAsync()
        {
            return await context.SaveChangesAsync() > 0;
        }
    }
}