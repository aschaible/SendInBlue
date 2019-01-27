using System.Collections.Generic;
using System.Linq;

namespace SendInBlue.Models
{
    public class UserModel
    {
        public string Email { get; set; }
        public Dictionary<string, string> Attributes { get; set; }
        public List<int> ListId { get; set; }

        public static UserModel Create(string email, Dictionary<string, string> attributes, params int[] lists)
        {
            var userModel = new UserModel
            {
                Email = email,
                Attributes = attributes,
                ListId = new List<int>()
            };

            if (lists != null && lists.Any())
            {
                userModel.ListId.AddRange(lists.Select(x => x).ToList());
            }

            return userModel;
        }
    }
}
