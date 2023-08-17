﻿namespace TheEstate.Models.UserModels
{
    public class UserModel
    {
        public string Id { get; set; }
        public string Firstname { get; set; } = string.Empty;
        public string Lastname { get; set; } = string.Empty;
        public string MobileNo { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
    }
}
