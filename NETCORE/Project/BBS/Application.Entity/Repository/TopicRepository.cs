﻿using Application.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Application.Core;
using Application.Core.Interface;
using Microsoft.EntityFrameworkCore;

namespace Application.Entity.Repository
{
    public class TopicRepository : Repository<Topic>, ITopicRepository
    {
        private readonly DataContext _dbContext;

        public TopicRepository(DataContext dbContext)
            : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public override Topic GetById(int id)
        {
            return _dbContext.Topics.Include(r => r.User).Include(r => r.Node).Include(r => r.LastReplyUser).FirstOrDefault(r => r.Id == id);
        }
        public override IEnumerable<Topic> GetList(Expression<Func<Topic, bool>> predicate)
        {
            return _dbContext.Topics.Include(r => r.Node).Where(predicate);
        }

        public Page<Topic> PageList(int pagesize = 20, int pageindex = 1)
        {
            return PageList(null, pagesize, pageindex);
        }

        public Page<Topic> PageList(Expression<Func<Topic, bool>> predicate, int pagesize = 20, int pageindex = 1)
        {
            var topics = _dbContext.Topics.Include(r => r.User).Include(r => r.Node).Include(r => r.LastReplyUser).AsQueryable().AsNoTracking();
            if (predicate != null)
            {
                topics = topics.Where(predicate);
            }
            var count = topics.Count();
            topics = topics.OrderByDescending(r => r.CreateOn)
                .OrderByDescending(r => r.Top)
                .Skip((pageindex - 1) * pagesize).Take(pagesize);
            return new Page<Topic>(topics.ToList(), pagesize, count);
        }
    }
}
