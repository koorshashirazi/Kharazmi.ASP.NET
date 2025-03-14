﻿using System;
using Mvc.Utility.Core.Managers.StateManager;

namespace Kharazmi.AspNetMvc.Core.Managers.StateManager
{
    internal static class ParameterConversion
    {
        public static object Unpack(object[] args, Type argType, int index)
        {
            Enforce.ArgumentNotNull(args, "args");

            if (args.Length <= index)
                throw new ArgumentException(string.Format(ParameterConversionResources.ArgOfTypeRequiredInPosition,
                    argType, index));

            var arg = args[index];

            if (arg != null && !argType.IsAssignableFrom(arg.GetType()))
                throw new ArgumentException(string.Format(ParameterConversionResources.WrongArgType, index,
                    arg.GetType(), argType));

            return arg;
        }

        public static TArg Unpack<TArg>(object[] args, int index)
        {
            return (TArg) Unpack(args, typeof(TArg), index);
        }

        public static void Validate(object[] args, Type[] expected)
        {
            if (args.Length > expected.Length)
                throw new ArgumentException(string.Format(ParameterConversionResources.TooManyParameters,
                    expected.Length, args.Length));

            for (var i = 0; i < expected.Length; ++i) Unpack(args, expected[i], i);
        }
    }
}