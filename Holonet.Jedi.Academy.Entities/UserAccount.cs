using System;
using System.Runtime.Serialization;

namespace Holonet.Jedi.Academy.Entities
{
    [DataContract]
    public class UserAccount : UserBase, IComparable<UserAccount>
    {
        [DataMember]
        public override string ShortName
        {
            get 
            {
                if (!string.IsNullOrEmpty(this.LastName) && !string.IsNullOrEmpty(this.FirstName))
                {
                    return string.Format("{0}, {1}", this.LastName, this.FirstName);
                }
                else if (!string.IsNullOrEmpty(this.LastName) && string.IsNullOrEmpty(this.FirstName))
                {
                    return this.LastName;
                }
                else if (string.IsNullOrEmpty(this.LastName) && !string.IsNullOrEmpty(this.FirstName))
                {
                    return this.FirstName;
                }
                else
                {
                    return this.UserId;
                }
            }
            set {  }
        }

        public UserAccount() {}

        public int CompareTo(UserAccount obj)
        {
            return this.ShortName.CompareTo(obj.ShortName);
        }
    }
}
