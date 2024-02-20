﻿using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;

namespace GitHubSimulator.Infrastructure.Configuration;
public class GuidSerializationProvider : IBsonSerializationProvider
{
    public IBsonSerializer GetSerializer(Type type)
    {
        if (type == typeof(Guid))
            return new GuidSerializer(GuidRepresentation.Standard);

        return null;
    }
}


