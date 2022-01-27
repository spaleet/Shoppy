﻿using CM.Domain.Comment;

namespace CM.Application.Comment.CommandHandles;

public class CancelCommentCommandHandler : IRequestHandler<CancelCommentCommand, Response<string>>
{
    #region Ctor

    private readonly IGenericRepository<Domain.Comment.Comment> _commentRepository;
    private readonly IMapper _mapper;

    public CancelCommentCommandHandler(IGenericRepository<Domain.Comment.Comment> commentRepository, IMapper mapper)
    {
        _commentRepository = Guard.Against.Null(commentRepository, nameof(_commentRepository));
        _mapper = Guard.Against.Null(mapper, nameof(_mapper));
    }

    #endregion

    public async Task<Response<string>> Handle(CancelCommentCommand request, CancellationToken cancellationToken)
    {
        var comment = await _commentRepository.GetEntityById(request.CommentId);

        if (comment is null)
            throw new NotFoundApiException();

        comment.State = CommentState.Canceled;

        _commentRepository.Update(comment);
        await _commentRepository.SaveChanges();

        return new Response<string>(ApplicationErrorMessage.RecordDeletedMessage);
    }
}