using Application.Core.Entity;
using Application.Core.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Application.Entity.Repository
{
    public class TopicReplyRepository : Repository<TopicReply>, ITopicReplyRepository
    {
        private readonly DataContext _dbContext;
        public TopicReplyRepository(DataContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public override IEnumerable<TopicReply> GetList(Expression<Func<TopicReply, bool>> predicate)
        {
            return _dbContext.TopicReplys.Include(r => r.ReplyUser).Where(predicate);
        }
    }
}
