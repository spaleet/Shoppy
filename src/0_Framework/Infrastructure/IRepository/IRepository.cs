﻿using _0_Framework.Application.Models.Paging;
using _0_Framework.Domain;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace _0_Framework.Infrastructure.IRepository;

public interface IRepository<TDocument>
    where TDocument : EntityBase
{
    IMongoQueryable<TDocument> AsQueryable(bool isDeletedFilter = true, CancellationToken cancellationToken = default);

    Task<IAsyncCursor<TDocument>> FindAsync(Expression<Func<TDocument, bool>> filter,
        CancellationToken cancellationToken = default);

    List<TDocument>? ApplyPagination(IMongoQueryable<TDocument> query, BasePaging pager,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(Expression<Func<TDocument, bool>> expression);

    Task<TDocument?> FindOne(FilterDefinition<TDocument> filter, CancellationToken cancellationToken = default);

    Task<HashSet<string>?> FullTextSearch(Expression<Func<TDocument, object>> field, string filter,
        CancellationToken cancellationToken = default);

    Task<List<TDocument>?> GetManyByFilter(FilterDefinition<TDocument> filter,
        CancellationToken cancellationToken = default);

    Task<TDocument?> FindByIdAsync(string id, CancellationToken cancellationToken = default);

    Task InsertAsync(TDocument document);

    Task UpdateAsync(TDocument document);

    Task DeleteAsync(string id);

    Task DeletePermanentAsync(string id);
}