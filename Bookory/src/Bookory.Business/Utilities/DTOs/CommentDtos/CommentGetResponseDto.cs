namespace Bookory.Business.Utilities.DTOs.CommentDtos
{
    public class CommentGetResponseDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Content { get; set; }
        public int Rating { get; set; }
        public List<CommentGetResponseDto> Replies { get; set; }

        public CommentGetResponseDto()
        {
            Replies = new List<CommentGetResponseDto>();
        }
    }
}
