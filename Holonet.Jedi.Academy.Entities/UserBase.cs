using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Holonet.Jedi.Academy.Entities
{
    [DataContract]
    public class UserBase
    {
        private string m_userId = string.Empty;
        [DataMember]
        public string UserId
        {
            get { return m_userId; }
            set { m_userId = value.StripHTMLTags().TrimIfNotNull(); }
        }

        private string m_fname = string.Empty;
        [DataMember]
        public string FirstName
        {
            get { return m_fname; }
            set { m_fname = value.StripHTMLTags().TrimIfNotNull(); }
        }

        private string m_lname = string.Empty;
        [DataMember]
        public string LastName
        {
            get { return m_lname; }
            set { m_lname = value.StripHTMLTags().TrimIfNotNull(); }
        }

        private string m_displayName = string.Empty;
        [DataMember]
        public string DisplayName
        {
            get { return m_displayName; }
            set { m_displayName = value.StripHTMLTags().TrimIfNotNull(); }
        }

        [DataMember]
        public virtual string ShortName
        {
            get { return string.Empty; }
            set { }
        }

        private string m_email = string.Empty;
        [DataMember]
        public string Email
        {
            get { return m_email; }
            set { m_email = value.StripHTMLTags().TrimIfNotNull(); }
        }

        private string m_phone = string.Empty;
        [DataMember]
        public string Phone
        {
            get { return m_phone; }
            set { m_phone = value.StripHTMLTags().TrimIfNotNull(); }
        }

        private string m_mobile = string.Empty;
        [DataMember]
        public string Mobile
        {
            get { return m_mobile; }
            set { m_mobile = value.StripHTMLTags().TrimIfNotNull(); }
        }

        private string m_fax = string.Empty;
        [DataMember]
        public string Fax
        {
            get { return m_fax; }
            set { m_fax = value.StripHTMLTags().TrimIfNotNull(); }
        }

        private string m_title = string.Empty;
        [DataMember]
        public string Title
        {
            get { return m_title; }
            set { m_title = value.StripHTMLTags().TrimIfNotNull(); }
        }

        private string m_company = string.Empty;
        [DataMember]
        public string Company
        {
            get { return m_company; }
            set { m_company = value.StripHTMLTags().TrimIfNotNull(); }
        }

        private string m_office = string.Empty;
        [DataMember]
        public string Office
        {
            get { return m_office; }
            set { m_office = value.StripHTMLTags().TrimIfNotNull(); }
        }

        private string m_department = string.Empty;
        [DataMember]
        public string Department
        {
            get { return m_department; }
            set { m_department = value.StripHTMLTags().TrimIfNotNull(); }
        }

        private string m_description = string.Empty;
        [DataMember]
        public string Description
        {
            get { return m_description; }
            set { m_description = value.StripHTMLTags().TrimIfNotNull(); }
        }

        public UserBase() { }
    }
}
