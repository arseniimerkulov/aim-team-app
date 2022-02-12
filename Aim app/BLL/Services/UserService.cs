﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BLL.Abstractions.Interfaces;
using Core;
using DAL.Abstractions.Interfaces;

namespace BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUser _currentUser;

        public UserService(IUnitOfWork unitOfWork, ICurrentUser currentUser)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<bool> LeaveRoom(Room room)
        {
            if (room == null)
            {
                return false;
            }

            room.Participants.Remove(
                room.Participants.FirstOrDefault(participant => participant.User.Id == _currentUser.User.Id));
            await _unitOfWork.RoomRepository.UpdateAsync(room);

            return true;
        }

        public async Task<bool> SwitchNotifications(Room room, bool stateOnOrOff)
        {
            if (room == null)
            {
                return false;
            }

            room.Participants.FirstOrDefault(participant => participant.User.Id == _currentUser.User.Id)!
                    .Notifications =
                stateOnOrOff;
            await _unitOfWork.RoomRepository.UpdateAsync(room);

            return true;
        }

        public bool IsUserVerified(User user)
        {
            return user.IsVerified;
        }

        public async Task<List<Room>> GetUserRooms()
        {
            var currentUser = _currentUser.User;

            Expression<Func<Room, Room>> selector = q => new Room()
            {
                Participants = q.Participants,
                RoomRoles = q.RoomRoles,
                TextChannels = q.TextChannels,
                BaseRole = q.BaseRole
            };

            var foundRooms = await _unitOfWork.RoomRepository.FindByConditionAsync(room =>
                room.Participants.Any(participant => participant.User.Id == currentUser.Id), Room.Selector);

            // var foundRooms = await _unitOfWork.RoomRepository
            //      .FindByConditionAsync(room =>
            //          room.Participants.Any(participant => participant.User.Id == currentUser.Id));

            return foundRooms.ToList();
        }

        public async Task ChangeUserNames(string firstName = null, string lastName = null)
        {
            var user = _currentUser.User;

            if (firstName != null)
            {
                user.FirstName = firstName;
            }

            if (lastName != null)
            {
                user.LastName = lastName;
            }

            await _unitOfWork.UserRepository.UpdateAsync(user);
        }

        public async Task<Role> GetRoleInRoom(Room room)
        {
            var currentUser = _currentUser.User;
            var role = room.Participants.FirstOrDefault(participantInfo => participantInfo.User.Id == currentUser.Id)
                ?.Role;

            return role;
        }

        public async Task<User> GetUserByUserNameOrEmail(string userName)
        {
            var users = await _unitOfWork.UserRepository.FindByConditionAsync(user =>
                user.UserName == userName || user.Email == userName);

            return users.FirstOrDefault();
        }
    }
}
