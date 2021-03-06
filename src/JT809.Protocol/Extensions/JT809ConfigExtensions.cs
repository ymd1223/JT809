﻿using JT809.Protocol.Enums;
using JT809.Protocol.Exceptions;
using JT809.Protocol.Formatters;
using JT809.Protocol.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace JT809.Protocol
{
    public static class JT809ConfigExtensions
    {
        private readonly static ConcurrentDictionary<string, JT809Serializer> jT809SerializerDict = new ConcurrentDictionary<string, JT809Serializer>(StringComparer.OrdinalIgnoreCase);

        public static object GetMessagePackFormatterByType(this IJT809Config jT809Config, Type type)
        {
            if (!jT809Config.FormatterFactory.FormatterDict.TryGetValue(type.GUID, out var formatter))
            {
                throw new JT809Exception(JT809ErrorCode.NotGlobalRegisterFormatterAssembly, type.FullName);
            }
            return formatter;
        }
        public static IJT809MessagePackFormatter<T> GetMessagePackFormatter<T>(this IJT809Config jT809Config)
        {
            return (IJT809MessagePackFormatter<T>)GetMessagePackFormatterByType(jT809Config, typeof(T));
        }
        public static JT809Serializer GetSerializer(this IJT809Config jT808Config)
        {
            if(!jT809SerializerDict.TryGetValue(jT808Config.ConfigId,out var serializer))
            {
                serializer = new JT809Serializer(jT808Config);
                jT809SerializerDict.TryAdd(jT808Config.ConfigId, serializer);
            }
            return serializer;
        }
    }
}
