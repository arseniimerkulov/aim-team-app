﻿using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Newtonsoft.Json;

namespace Core
{
    public class Room : BaseEntity
    {
        public string RoomName { get; set; }

        public string RoomDescription { get; set; }

        public IList<ParticipantInfo> Participants { get; set; }

        public IList<Role> RoomRoles { get; set; }

        public IList<TextChannel> TextChannels { get; set; }

        public Role BaseRole { get; set; }

        [JsonIgnore]
        [NotMapped]
        public ReadOnlyCollection<byte> Photo { get; set; }

        public string PhotoSource
        {
            get
            {
                if (this.Photo != null)
                {
                    return Convert.ToBase64String(this.Photo.ToArray());
                }

                return string.Empty;
            }
            set { this.Photo = Array.AsReadOnly(Convert.FromBase64String(value)); }
        }
    }
}
