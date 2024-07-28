using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Comments;
using api.Dtos.Posts;
using api.Models;

namespace api.Mappers
{
    public static class PostMappers
    {
        public static PostDto ToPostDto(this Post post)
        {
            return new PostDto
            {
                Id = post.Id,
                Content = post.Content,
                Author = {
                    UserId = post.BlogUser.Id,
                    UserName = post.BlogUser.UserName ?? "unknown"
                },
                Title = post.Title,
                CreatedAt = post.CreatedAt,
                UpdatedAt = post.UpdatedAt,
                Tags = post.PostTags.Select(pt => pt.Tag.Name).ToList(),
                Comments = post.Comments.Select(c => c.ToCommentDto()).ToList()
            };
        }

        public static CommentDto ToCommentDto(this Comment comment)
        {
            return new CommentDto
            {
                Id = comment.Id,
                Content = comment.Content,
                CreatedAt = comment.CreatedAt,
                UserName = comment.BlogUser.UserName ?? "unknown"
            };
        }

        public static Comment CreateDtoToComment(this CreateCommentDto createCommentDto)
        {
            return new Comment
            {
                BlogUserId = createCommentDto.BlogUserId,
                Content = createCommentDto.Content,
                PostId = createCommentDto.PostId,
                CreatedAt = DateTime.Now.ToUniversalTime()
            };
        }

        public static Post ToPostFromCreateDto(this CreatePostDto createDto)
        {
            return new Post
            {
                Title = createDto.Title,
                Content = createDto.Content
            };
        }
    }
}