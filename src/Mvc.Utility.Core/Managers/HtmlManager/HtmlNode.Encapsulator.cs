﻿using System.Collections;
using System.Collections.Generic;
using System.Reflection;
#if !METRO && !NETSTANDARD1_3
using System;

namespace Mvc.Utility.Core.Managers.HtmlManager
{
    public partial class HtmlNode
    {
        /// <summary>
        ///     Fill an object and go through it's properties and fill them too.
        /// </summary>
        /// <typeparam name="T">Type of object to want to fill. It should have atleast one property that defined XPath.</typeparam>
        /// <param name="htmlDocument">
        ///     If htmlDocument includes data , leave this parameter null. Else pass your specific
        ///     htmldocument.
        /// </param>
        /// <returns>Returns an object of type T including Encapsulated data.</returns>
        public T GetEncapsulatedData<T>(HtmlDocument htmlDocument = null)
        {
            return (T) GetEncapsulatedData(typeof(T), htmlDocument);
        }

        /// <summary>
        ///     Fill an object and go through it's properties and fill them too.
        /// </summary>
        /// <param name="targetType">Type of object to want to fill. It should have atleast one property that defined XPath.</param>
        /// <param name="htmlDocument">
        ///     If htmlDocument includes data , leave this parameter null. Else pass your specific
        ///     htmldocument.
        /// </param>
        /// <returns>Returns an object of type targetType including Encapsulated data.</returns>
        public object GetEncapsulatedData(Type targetType, HtmlDocument htmlDocument = null)
        {
            #region SettingPrerequisite

            if (targetType == null) throw new ArgumentNullException("Parameter targetType is null");

            HtmlDocument source = null;

            if (htmlDocument == null)
                source = OwnerDocument;
            else
                source = htmlDocument;

            object targetObject;

            if (targetType.IsInstantiable() == false
            ) // if it can not create instanse of T because of lack of constructor in type T.
                throw new MissingMethodException("Parameterless Constructor excpected for " + targetType.FullName);

            targetObject = Activator.CreateInstance(targetType);

            #endregion SettingPrerequisite

            #region targetObject_Defined_XPath

            if (targetType.IsDefinedAttribute(typeof(HasXPathAttribute))
            ) // Object has xpath attribute (Defined HasXPath)
            {
                // Store list of properties that defined xpath attribute
                var validProperties = targetType.GetPropertiesDefinedXPath();
                if (validProperties.CountOfIEnumerable() == 0
                ) // if no XPath property exist in type T while T defined HasXpath attribute.
                    throw new MissingXPathException("Type " + targetType.FullName +
                                                    " defined HasXPath Attribute but it does not have any property with XPath Attribte.");

                // Fill targetObject variable Properties ( T targetObject )
                foreach (var propertyInfo in validProperties)
                {
                    // Get xpath attribute from valid properties
                    // for .Net old versions:
                    var xPathAttribute =
                        (propertyInfo.GetCustomAttributes(typeof(XPathAttribute), false) as IList)[0] as XPathAttribute;

                    #region Property_IsNOT_IEnumerable

                    if (propertyInfo.IsIEnumerable() == false) // Property is None-IEnumerable
                    {
                        HtmlNode htmlNode = null;

                        // try to fill htmlNode based on XPath given
                        try
                        {
                            htmlNode = source.DocumentNode.SelectSingleNode(xPathAttribute.XPath);
                        }
                        catch // if it can not select node based on given xpath
                        {
                            throw new NodeNotFoundException("Cannot find node with giving XPath to bind to " +
                                                            propertyInfo.PropertyType.FullName + " " +
                                                            propertyInfo.Name);
                        }

                        if (htmlNode == null)
                            throw new NodeNotFoundException("Cannot find node with givig XPath to bind to " +
                                                            propertyInfo.PropertyType.FullName + " " +
                                                            propertyInfo.Name);

                        #region Property_Is_HasXPath_UserDefinedClass

                        // Property is None-IEnumerable HasXPath-user-defined class
                        if (propertyInfo.PropertyType.IsDefinedAttribute(typeof(HasXPathAttribute)))
                        {
                            var innerHtmlDocument = new HtmlDocument();

                            innerHtmlDocument.LoadHtml(htmlNode.InnerHtml);

                            var o = GetEncapsulatedData(propertyInfo.PropertyType, innerHtmlDocument);

                            propertyInfo.SetValue(targetObject, o, null);
                        }

                        #endregion Property_Is_HasXPath_UserDefinedClass

                        #region Property_Is_SimpleType

                        // Property is None-IEnumerable value-type or .Net class or user-defined class.
                        // AND does not deifned xpath and shouldn't have property that defined xpath.
                        else
                        {
                            var result = string.Empty;

                            if (xPathAttribute.AttributeName == null) // It target None-IEnumerable value of HTMLTag
                                result = Tools.GetNodeValueBasedOnXPathReturnType<string>(htmlNode, xPathAttribute);
                            else // It target None-IEnumerable attribute of HTMLTag
                                result = htmlNode.GetAttributeValue(xPathAttribute.AttributeName, null);

                            if (result == null)
                                throw new NodeAttributeNotFoundException(
                                    "Can not find " + xPathAttribute.AttributeName + " Attribute in " + htmlNode.Name +
                                    " related to " + propertyInfo.PropertyType.FullName + " " + propertyInfo.Name);

                            object resultCastedToTargetPropertyType;

                            try
                            {
                                resultCastedToTargetPropertyType =
                                    Convert.ChangeType(result, propertyInfo.PropertyType);
                            }
                            catch (FormatException)
                            {
                                throw new FormatException("Can not convert Invalid string to " +
                                                          propertyInfo.PropertyType.FullName + " " + propertyInfo.Name);
                            }
                            catch (Exception ex)
                            {
                                throw new Exception("Unhandled Exception : " + ex.Message);
                            }

                            propertyInfo.SetValue(targetObject, resultCastedToTargetPropertyType, null);
                        }

                        #endregion Property_Is_SimpleType
                    }

                    #endregion Property_IsNOT_IEnumerable

                    #region Property_Is_IEnumerable

                    else // Property is IEnumerable<T>
                    {
                        var T_Types = propertyInfo.GetGenericTypes() as IList<Type>; // Get T type

                        if (T_Types == null || T_Types.Count == 0)
                            throw new ArgumentException(propertyInfo.Name + " should have one generic argument.");

                        if (T_Types.Count > 1)
                            throw new ArgumentException(propertyInfo.Name + " should have one generic argument.");

                        if (T_Types.Count == 1) // It is NOT something like Dictionary<Tkey , Tvalue>
                        {
                            HtmlNodeCollection nodeCollection;

                            // try to fill nodeCollection based on given xpath.
                            try
                            {
                                nodeCollection = source.DocumentNode.SelectNodes(xPathAttribute.XPath);
                            }
                            catch
                            {
                                throw new NodeNotFoundException(
                                    "Cannot find node with givig XPath to bind to " +
                                    propertyInfo.PropertyType.FullName + " " + propertyInfo.Name);
                            }

                            if (nodeCollection == null || nodeCollection.Count == 0)
                                throw new NodeNotFoundException(
                                    "Cannot find node with givig XPath to bind to " +
                                    propertyInfo.PropertyType.FullName + " " + propertyInfo.Name);

                            var result = T_Types[0].CreateIListOfType();

                            #region Property_Is_IEnumerable<HasXPath-UserDefinedClass>

                            if (T_Types[0].IsDefinedAttribute(typeof(HasXPathAttribute))
                            ) // T is IEnumerable HasXPath-user-defined class (T type Defined XPath properties)
                            {
                                foreach (var node in nodeCollection)
                                {
                                    var innerHtmlDocument = new HtmlDocument();
                                    innerHtmlDocument.LoadHtml(node.InnerHtml);

                                    var o = GetEncapsulatedData(T_Types[0], innerHtmlDocument);

                                    result.Add(o);
                                }
                            }

                            #endregion Property_Is_IEnumerable<HasXPath-UserDefinedClass>

                            #region Property_Is_IEnumerable<SimpleClass>

                            else // T is value-type or .Net class or user-defined class ( without xpath )
                            {
                                if (xPathAttribute.AttributeName == null) // It target value
                                    try
                                    {
                                        result = Tools.GetNodesValuesBasedOnXPathReturnType(nodeCollection,
                                            xPathAttribute, T_Types[0]);
                                    }
                                    catch (FormatException)
                                    {
                                        throw new FormatException(
                                            "Can not convert Invalid string in node collection to " +
                                            T_Types[0].FullName + " " + propertyInfo.Name);
                                    }
                                    catch (Exception ex)
                                    {
                                        throw new Exception("Unhandled Exception : " + ex.Message);
                                    }
                                else // It target attribute
                                    foreach (var node in nodeCollection)
                                    {
                                        var nodeAttributeValue =
                                            node.GetAttributeValue(xPathAttribute.AttributeName, null);
                                        if (nodeAttributeValue == null)
                                            throw new NodeAttributeNotFoundException(
                                                "Can not find " + xPathAttribute.AttributeName + " Attribute in " +
                                                node.Name + " related to " + propertyInfo.PropertyType.FullName + " " +
                                                propertyInfo.Name);

                                        object resultCastedToTargetPropertyType;

                                        try
                                        {
                                            resultCastedToTargetPropertyType =
                                                Convert.ChangeType(nodeAttributeValue, T_Types[0]);
                                        }
                                        catch (FormatException
                                        ) // if it can not cast result(string) to type of property.
                                        {
                                            throw new FormatException(
                                                "Can not convert Invalid string to " + T_Types[0].FullName + " " +
                                                propertyInfo.Name);
                                        }
                                        catch (Exception ex)
                                        {
                                            throw new Exception("Unhandled Exception : " + ex.Message);
                                        }

                                        result.Add(resultCastedToTargetPropertyType);
                                    }
                            }

                            #endregion Property_Is_IEnumerable<SimpleClass>

                            if (result == null || result.Count == 0)
                                throw new Exception("Cannot fill " + propertyInfo.PropertyType.FullName + " " +
                                                    propertyInfo.Name + " because it is null.");

                            propertyInfo.SetValue(targetObject, result, null);
                        }
                    }

                    #endregion Property_Is_IEnumerable
                }

                return targetObject;
            }

            #endregion targetObject_Defined_XPath

            #region targetObject_NOTDefined_XPath

            throw new MissingXPathException(
                "Type T must define HasXPath attribute and include properties with XPath attribute.");

            #endregion targetObject_NOTDefined_XPath
        }
    }

    /// <summary>
    ///     Includes tools that GetEncapsulatedData method uses them.
    /// </summary>
    internal static class Tools
    {
        /// <summary>
        ///     Determine if a type define an attribute or not , supporting both .NetStandard and .NetFramework2.0
        /// </summary>
        /// <param name="type">Type you want to test it.</param>
        /// <param name="attributeType">Attribute that type must have or not.</param>
        /// <returns>If true , The type parameter define attributeType parameter.</returns>
        internal static bool IsDefinedAttribute(this Type type, Type attributeType)
        {
            if (type == null)
                throw new ArgumentNullException(
                    "Parameter type is null when checking type defined attributeType or not.");

            if (attributeType == null)
                throw new ArgumentNullException(
                    "Parameter attributeType is null when checking type defined attributeType or not.");

#if !(NETSTANDARD1_3 || NETSTANDARD1_6)
            if (type.IsDefined(attributeType, false)) return true;

            return false;
#endif

#if NETSTANDARD1_3 || NETSTANDARD1_6
            if (type.GetTypeInfo().IsDefined(attributeType) == true)
            {
                return true;
            }
            else
            {
                return false;
            }
#endif

            throw new NotImplementedException("Can't Target any platform when checking " + type.FullName + " is a " +
                                              attributeType.FullName + " or not.");
        }

        /// <summary>
        ///     Retrive properties of type that defined <see cref="XPathAttribute" />.
        /// </summary>
        /// <param name="type">Type that you want to find it's XPath-Defined properties.</param>
        /// <returns>IEnumerable of property infos of a type , that defined specific attribute.</returns>
        internal static IEnumerable<PropertyInfo> GetPropertiesDefinedXPath(this Type type)
        {
            if (type == null)
                throw new ArgumentNullException(
                    "Parameter type is null while retrieving properties defined XPathAttribute of Type type.");

            PropertyInfo[] properties = null;

#if !(NETSTANDARD1_3 || NETSTANDARD1_6)
            properties = type.GetProperties();
#endif

#if NETSTANDARD1_3 || NETSTANDARD1_6
            properties = type.GetTypeInfo().GetProperties();
#endif

            return properties.HAPWhere(x => x.IsDefined(typeof(XPathAttribute), false));

            throw new NotImplementedException(
                "Can't Target any platform while retrieving properties defined XPathAttribute of Type type.");
        }

        /// <summary>
        ///     Determine if a <see cref="PropertyInfo" /> has implemented <see cref="IEnumerable" /> BUT <see cref="string" /> is
        ///     considered as NONE-IEnumerable !
        /// </summary>
        /// <param name="propertyInfo">The property info you want to test.</param>
        /// <returns>True if property info is IEnumerable.</returns>
        internal static bool IsIEnumerable(this PropertyInfo propertyInfo)
        {
            //return propertyInfo.PropertyType.GetInterface(typeof(IEnumerable<>).FullName) != null;

            if (propertyInfo == null)
                throw new ArgumentNullException(
                    "Parameter propertyInfo is null while checking propertyInfo for being IEnumerable or not.");

            if (propertyInfo.PropertyType == typeof(string)) return false;
#if !(NETSTANDARD1_3 || NETSTANDARD1_6)
            return typeof(IEnumerable).IsAssignableFrom(propertyInfo.PropertyType);
#endif

#if NETSTANDARD1_3 || NETSTANDARD1_6
                return typeof(IEnumerable).GetTypeInfo().IsAssignableFrom(propertyInfo.PropertyType);
#endif

            throw new NotImplementedException(
                "Can't Target any platform while checking propertyInfo for being IEnumerable or not.");
        }

        /// <summary>
        ///     Returns T type(first generic type) of <see cref="IEnumerable{T}" /> or <see cref="List{T}" />.
        /// </summary>
        /// <param name="propertyInfo">IEnumerable-Implemented property</param>
        /// <returns>List of generic types.</returns>
        internal static IEnumerable<Type> GetGenericTypes(this PropertyInfo propertyInfo)
        {
            if (propertyInfo == null)
                throw new ArgumentNullException(
                    "Parameter propertyInfo is null while Getting generic types of Property.");

#if !(NETSTANDARD1_3 || NETSTANDARD1_6)
            return propertyInfo.PropertyType.GetGenericArguments();
#endif

#if NETSTANDARD1_3 || NETSTANDARD1_6
            return propertyInfo.PropertyType.GetTypeInfo().GetGenericArguments();
#endif

            throw new NotImplementedException("Can't Target any platform while Getting generic types of Property.");
        }

        /// <summary>
        ///     Find and Return a mehtod that defined in a class by it's name.
        /// </summary>
        /// <param name="type">Type of class include requested method.</param>
        /// <param name="methodName">Name of requested method as string.</param>
        /// <returns>Method info of requested method.</returns>
        internal static MethodInfo GetMethodByItsName(this Type type, string methodName)
        {
            if (type == null) throw new ArgumentNullException("Parameter type is null while Getting method from it.");

            if (methodName == null || methodName == "")
                throw new ArgumentNullException("Parameter methodName is null while Getting method from Type type.");

#if !(NETSTANDARD1_3 || NETSTANDARD1_6)
            return type.GetMethod(methodName);
#endif

#if NETSTANDARD1_3 || NETSTANDARD1_6
            return type.GetTypeInfo().GetMethod(methodName);
#endif

            throw new NotImplementedException(
                "Can't Target any platform while getting Method methodName from Type type.");
        }

        /// <summary>
        ///     Create <see cref="IList" /> of given type.
        /// </summary>
        /// <param name="type">Type that you want to make a List of it.</param>
        /// <returns>Returns IList of given type.</returns>
        internal static IList CreateIListOfType(this Type type)
        {
            if (type == null) throw new ArgumentNullException("Parameter type is null while creating List<type>.");

            var listType = typeof(List<>);
            var constructedListType = listType.MakeGenericType(type);
            return Activator.CreateInstance(constructedListType) as IList;
        }

        /// <summary>
        ///     Returns the part of value of <see cref="Mvc.Utility.Core.Managers.HtmlManager.HtmlNode" /> you want as .
        /// </summary>
        /// <param name="htmlNode">A htmlNode instance.</param>
        /// <param name="xPathAttribute">Attribute that includes ReturnType</param>
        /// <returns>String that choosen from HtmlNode as result.</returns>
        internal static T GetNodeValueBasedOnXPathReturnType<T>(HtmlNode htmlNode, XPathAttribute xPathAttribute)
        {
            if (htmlNode == null) throw new ArgumentNullException("parameter html node is null");

            if (xPathAttribute == null) throw new ArgumentNullException("parameter xpathAttribute is null");

            object result;
            var TType = typeof(T);

            switch (xPathAttribute.NodeReturnType)
            {
                case ReturnType.InnerHtml:
                {
                    result = Convert.ChangeType(htmlNode.InnerHtml, TType);
                }
                    break;

                case ReturnType.InnerText:
                {
                    result = Convert.ChangeType(htmlNode.InnerText, TType);
                }
                    break;

                case ReturnType.OuterHtml:
                {
                    result = Convert.ChangeType(htmlNode.OuterHtml, TType);
                }
                    break;

                default: throw new NotImplementedException();
            }

            return (T) result;
        }

        /// <summary>
        ///     Returns parts of values of <see cref="Mvc.Utility.Core.Managers.HtmlManager.HtmlNode" /> you want as
        ///     <see cref="IList{T}" />.
        /// </summary>
        /// <param name="htmlNodeCollection">
        ///     <see cref="HtmlNodeCollection" /> that you want to retrive each
        ///     <see cref="Mvc.Utility.Core.Managers.HtmlManager.HtmlNode" /> value.
        /// </param>
        /// <param name="xPathAttribute">A <see cref="XPathAttribute" /> instnce incules <see cref="ReturnType" />.</param>
        /// <param name="listGenericType">Type of IList generic you want.</param>
        /// <returns></returns>
        internal static IList GetNodesValuesBasedOnXPathReturnType(HtmlNodeCollection htmlNodeCollection
            , XPathAttribute xPathAttribute, Type listGenericType)
        {
            if (htmlNodeCollection == null || htmlNodeCollection.Count == 0)
                throw new ArgumentNullException("parameter htmlNodeCollection is null or empty.");

            if (xPathAttribute == null) throw new ArgumentNullException("parameter xpathAttribute is null");

            var result = listGenericType.CreateIListOfType();

            switch (xPathAttribute.NodeReturnType)
            {
                case ReturnType.InnerHtml:
                {
                    foreach (var node in htmlNodeCollection)
                        result.Add(Convert.ChangeType(node.InnerHtml, listGenericType));
                }
                    break;

                case ReturnType.InnerText:
                {
                    foreach (var node in htmlNodeCollection)
                        result.Add(Convert.ChangeType(node.InnerText, listGenericType));
                }
                    break;

                case ReturnType.OuterHtml:
                {
                    foreach (var node in htmlNodeCollection)
                        result.Add(Convert.ChangeType(node.OuterHtml, listGenericType));
                }
                    break;
            }

            return result;
        }

        /// <summary>
        ///     This method works like Where method in LINQ.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        internal static IEnumerable<TSource> HAPWhere<TSource>(this IEnumerable<TSource> source
            , HAPFunc<TSource, bool> predicate)
        {
            foreach (var item in source)
                if (predicate(item))
                    yield return item;
        }

        /// <summary>
        ///     Check if the type can instantiated.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        internal static bool IsInstantiable(this Type type)
        {
            if (type == null) throw new ArgumentNullException("type is null");

#if !(NETSTANDARD1_3 || NETSTANDARD1_6)
            // checking for having parameterless constructor.
            if (type.GetConstructor(Type.EmptyTypes) == null) return false;

            return true;
#endif

#if NETSTANDARD1_3 || NETSTANDARD1_6
// checking for having parameterless constructor.
            if (type.GetTypeInfo().DeclaredConstructors.HAPWhere(x => x.GetParameters().Length == 0).CountOfIEnumerable() == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
#endif

            throw new NotImplementedException(
                "Can't Target any platform while getting Method methodName from Type type.");
        }

        /// <summary>
        ///     Returns count of elements stored in IEnumerable of T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        internal static int CountOfIEnumerable<T>(this IEnumerable<T> source)
        {
            if (source == null)
                throw new ArgumentNullException("Parameter source is null while counting the IEnumerable");

            var counter = 0;
            foreach (var item in source) counter++;

            return counter;
        }

        /// <summary>
        ///     Simulate Func method to use in Lambada Expression.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="arg"></param>
        /// <returns></returns>
        internal delegate TResult HAPFunc<T, TResult>(T arg);
    }

    /// <summary>
    ///     Specify which part of <see cref="Mvc.Utility.Core.Managers.HtmlManager.HtmlNode" /> is requested.
    /// </summary>
    public enum ReturnType
    {
        InnerText,
        InnerHtml,
        OuterHtml
    }

    /// <summary>
    ///     Just mark and flag classes to show they have properties that defined <see cref="XPathAttribute" />.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class HasXPathAttribute : Attribute
    {
    }

    /// <summary>
    ///     Includes XPath and <see cref="NodeReturnType" />. XPath for finding html tags and <see cref="NodeReturnType" /> for
    ///     specify which part of <see cref="Mvc.Utility.Core.Managers.HtmlManager.HtmlNode" /> you want to return.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class XPathAttribute : Attribute
    {
        public XPathAttribute(string xpathString, ReturnType nodeReturnType = ReturnType.InnerText)
        {
            XPath = xpathString;
            NodeReturnType = nodeReturnType;
        }

        public XPathAttribute(string xpathString, string attributeName)
        {
            XPath = xpathString;
            AttributeName = attributeName;
        }

        public string XPath { get; }
        public string AttributeName { get; set; }
        public ReturnType NodeReturnType { get; set; }
    }

    public class NodeNotFoundException : Exception
    {
        public NodeNotFoundException()
        {
        }

        public NodeNotFoundException(string message) : base(message)
        {
        }

        public NodeNotFoundException(string message, Exception inner) : base(message, inner)
        {
        }
    }

    public class NodeAttributeNotFoundException : Exception
    {
        public NodeAttributeNotFoundException()
        {
        }

        public NodeAttributeNotFoundException(string message) : base(message)
        {
        }

        public NodeAttributeNotFoundException(string message, Exception inner) : base(message, inner)
        {
        }
    }

    public class MissingXPathException : Exception
    {
        public MissingXPathException()
        {
        }

        public MissingXPathException(string message) : base(message)
        {
        }

        public MissingXPathException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}

#if FX20
namespace System.Runtime.CompilerServices
{
    [AttributeUsage(AttributeTargets.Method |
    AttributeTargets.Class | AttributeTargets.Assembly)]
    public sealed class ExtensionAttribute : Attribute
    {
    }
}
#endif
#endif