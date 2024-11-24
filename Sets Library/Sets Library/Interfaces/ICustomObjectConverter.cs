﻿/*
 * File: ICustomObjectConverter.cs
 * Author: Phiwokwakhe Khathwane
 * Date: 24 November 2024
 * 
 * Description:
 * This file contains the definition of the ICustomObjectConverter interface, 
 * which establishes a contract for converting a string representation of an 
 * object to an instance of type T. This interface is intended to be used in 
 * scenarios where custom object conversion is required for use within sets.
 * 
 * Key Features:
 * - Defines a method, ToObject, to convert a string representation to an object.
 * - Supports generic types that implement the IComparable interface.
 * - Utilizes SetExtractionSettings<T> for customizing the conversion process.
 */
using SetsLibrary.Models;
namespace SetLibrary.Models
{
    /// <summary>
    /// Defines a contract for converting a string to an object of type T, 
    /// which can be utilized within a set.
    /// </summary>
    /// <typeparam name="T">The type of objects to convert, which must implement <see cref="IComparable"/>.</typeparam>
    public interface ICustomObjectConverter<T>
        where T : IComparable<T>
    {
        /// <summary>
        /// Converts a string representation of an object to an instance of type T.
        /// </summary>
        /// <param name="field">The string representation of the object to convert.</param>
        /// <param name="settings">The extraction settings used for the conversion process.</param>
        /// <returns>An instance of type T created from the provided string representation.</returns>
        T ToObject(string field, SetExtractionSettings<T> settings);
    } // interface
} // namespace
