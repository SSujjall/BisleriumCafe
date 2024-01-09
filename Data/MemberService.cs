using System.Text.Json;

namespace BisleriumCafe.Data
{
    public static class MemberService
    {
        public static List<Member> GetMemberData()
        {
            string memberFilePath = Utils.GetMemberPath();

            if (!File.Exists(memberFilePath))
            {
                return new List<Member>();
            }

            var json = File.ReadAllText(memberFilePath);
            var members = JsonSerializer.Deserialize<List<Member>>(json);
            return members;
        }

        public static void SaveMemberData(List<Member> members)
        {
            string memberFilePath = Utils.GetMemberPath();

            var json = JsonSerializer.Serialize(members);
            File.WriteAllText(memberFilePath, json);
        }

        public static List<Member> CreateNewMember(long userId)
        {


            List<Member> members = GetMemberData();
            bool userExists = members.Any(x => x.MemberID == userId);

            if (!userExists)
            {
                members.Add(new Member()
                {
                    MemberID = userId,
                    UpdatedNumberOfOrders = 1
                });
            }
            SaveMemberData(members);
            return members;
        }
    }
}
