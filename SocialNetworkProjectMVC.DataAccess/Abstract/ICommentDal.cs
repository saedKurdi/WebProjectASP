﻿using SocialNetworkProjectMVC.Core.DataAccess.Abstract;
using SocialNetworkProjectMVC.Entities.Models;

namespace SocialNetworkProjectMVC.DataAccess.Abstract;
public interface ICommentDal : IEntityRepository<Comment> { }