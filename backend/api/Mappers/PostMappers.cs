using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
                // Tags = post.Tags,
                Title = post.Title
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