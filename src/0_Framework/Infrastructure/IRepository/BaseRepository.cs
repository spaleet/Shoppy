﻿using _0_Framework.Application.Exceptions;
using _0_Framework.Application.Models.Paging;
using _0_Framework.Domain;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace _0_Framework.Infrastructure.IRepository;

public class BaseRepository<TDocument, TSettings> : IRepository<TDocument>
    where TDocument : EntityBase
    where TSettings : BaseDbSettings
{
    #region Ctor

    private readonly IMongoCollection<TDocument> _collection;
    private readonly TSettings _settings;

    public BaseRepository(IOptionsSnapshot<TSettings> settings)
    {
        _settings = settings.Value;

        _collection = DbConnection.Conncet<TDocument>(_settings);
    }

    #endregion

    #region GetQuery

    public IMongoQueryable<TDocument> AsQueryable(bool isDeletedFilter = true, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (!isDeletedFilter)
            return _collection.AsQueryable();

        return _collection.AsQueryable().Where(x => x.IsDeleted == false);
    }

    #endregion

    #region GetPagination

    public List<TDocument> ApplyPagination(IMongoQueryable<TDocument> query, BasePaging pager, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return query
                .DocumentPaging(pager)
                .ToListSafe();
    }

    #endregion

    #region Exists

    public async Task<bool> ExistsAsync(Expression<Func<TDocument, bool>> expression)
    {
        return await _collection.AsQueryable().AnyAsync(expression);
    }

    #endregion

    #region GetByFilter

    public async Task<TDocument> GetByFilter(FilterDefinition<TDocument> filter, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var res = await _collection.FindAsync(filter);

        var document = await res.FirstOrDefaultAsync();

        if (document is null)
            throw new NotFoundApiException();

        return document;
    }

    #endregion

    #region GetByIdAsync

    public async Task<TDocument> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var filter = MongoDbFilters<TDocument>.GetByIdFilter(id);

        var res = await _collection.FindAsync(filter);

        var document = await res.FirstOrDefaultAsync();

        if (document is null)
            throw new NotFoundApiException();

        return document;
    }

    #endregion

    #region Insert

    public async Task InsertAsync(TDocument document)
    {
        await _collection.InsertOneAsync(document);
    }

    #endregion

    #region Update

    public async Task UpdateAsync(TDocument document)
    {
        document.LastUpdateDate = DateTime.UtcNow;

        var filter = MongoDbFilters<TDocument>.GetByIdFilter(document.Id);

        await _collection.ReplaceOneAsync(filter, document);
    }

    #endregion

    #region Delete

    public async Task DeleteAsync(string id)
    {
        var document = await GetByIdAsync(id);

        document.IsDeleted = true;

        await UpdateAsync(document);
    }

    #endregion

    #region DeletePermanentAsync

    public async Task DeletePermanentAsync(string id)
    {
        var document = await GetByIdAsync(id);

        var filter = MongoDbFilters<TDocument>.GetByIdFilter(document.Id);

        await _collection.DeleteOneAsync(filter);
    }

    #endregion
}