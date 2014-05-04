using Classfinder.Database;
using Microsoft.AspNet.SignalR;
using System.Collections.Generic;
using System.Linq;

namespace Classfinder.Hubs
{
    public class ClassInfo : Hub
    {
        public dynamic[] GetPeers(string Username)
        {
            var Retting = new List<dynamic>();
            using (CfDb db = new CfDb())
            {
                var user = db.Users.FirstOrDefault(u => u.Username == Username);
                Retting.Add(user.Realname);
                if (user != null)
                {
                    if (user.FirstSem != null)
                    {
                        var FirstSem = user.FirstSem.ToArray();
                        Retting.Add(user.FirstSem.Select(a => new
                        {
                            IsFirst = true,
                            a.Period,
                            a.Id,
                            a.Teacher,
                            a.Name,
                            Peers = a.FirstSemStudents.Select(u => new
                            {
                                u.Realname,
                                u.Username
                            }).ToArray()
                        }).OrderBy(a => a.Period).ToArray());
                    }
                    if (user.SecondSem != null)
                    {
                        Retting.Add(user.SecondSem.Select(a => new
                            {
                                IsFirst = false,
                                a.Period,
                                a.Id,
                                a.Teacher,
                                a.Name,
                                Peers = a.SecondSemStudents.Select(u => new
                                {
                                    u.Realname,
                                    u.Username
                                }).ToArray()
                            }).OrderBy(a => a.Period).ToArray());
                    }
                    var Res = Retting.ToArray();
                    return Res;
                }
            }
            return null;
        }
    }
}