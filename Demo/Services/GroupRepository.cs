using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JORB;

namespace Demo.Services
{
    public interface IGroupRepository : IRepository<RuleGroup, string>
    {
    }

    public class GroupRepository : IGroupRepository
    {
        private static Dictionary<string, RuleGroup> _groups = new Dictionary<string, RuleGroup>();

        internal static void Seed()
        {
            RuleGroup group = new RuleGroup("group", null);
            _groups["group"] = group;
        }

        public IQueryable<RuleGroup> GetAll()
        {
            return _groups.Values.AsQueryable();
        }

        public RuleGroup GetFor(string param)
        {
            return _groups[param];
        }

        public void Add(RuleGroup t)
        {
            _groups[t.GroupName] = t;
        }

        public void Remove(RuleGroup t)
        {
            _groups.Remove(t.GroupName);
        }
    }
}
