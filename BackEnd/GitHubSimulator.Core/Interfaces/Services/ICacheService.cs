﻿using GitHubSimulator.Core.Models.Cache;

namespace GitHubSimulator.Core.Interfaces.Services;

public interface ICacheService
{
    T GetData<T>(string key);
    bool SetData<T>(string key, T value, DateTimeOffset expirationTime);
    object RemoveData(string key);
    Task<SearchResult> SearchAllIndexesAsync(string searchTerm);
}
