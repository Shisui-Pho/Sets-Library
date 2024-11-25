﻿/*
 * File: SetExtractionConfiguration.cs
 * Author: Phiwokwakhe Khathwane
 * Date: 25 November 2024
 * 
 * Description:
 * Defines the SetExtractionConfiguration class, which specifies the configuration
 * for extracting sets, including terminators for fields and rows, and an optional 
 * custom converter for converting string literals into objects.
 * 
 * Key Features:
 * - Supports configuration with field and row terminators.
 * - Allows integration of a custom object converter implementing ICustomObjectConverter<T>.
 * - Includes validation to ensure terminators are not null and distinct.
 * - Provides a method to convert string records into objects using the provided converter.
 */

using SetLibrary.Models;
using System;

namespace SetsLibrary.Models;

/// <summary>
/// Represents the configuration for extracting sets, including terminators and an optional custom converter.
/// </summary>
/// <typeparam name="T">The type of object that the extracted records will be converted into. This type must implement <see cref="IComparable{T}"/>.</typeparam>
public class SetExtractionConfiguration<T>
    where T : IComparable<T>
{
    /// <summary>
    /// Gets the field terminator used to separate fields in a record.
    /// </summary>
    public string FieldTerminator { get; private set; }

    /// <summary>
    /// Gets the row terminator used to separate rows in the data.
    /// </summary>
    public string RowTerminator { get; private set; }

    /// <summary>
    /// Gets the optional custom converter used to convert a string literal into an object of type <typeparamref name="T"/>.
    /// </summary>
    public ICustomObjectConverter<T>? Converter { get; private set; }

    /// <summary>
    /// Gets a value indicating whether a custom object converter is provided.
    /// </summary>
    public bool IsICustomObject { get; private set; }

    // Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="SetExtractionConfiguration{T}"/> class with field and row terminators.
    /// </summary>
    /// <param name="fieldTerminator">The string used to separate fields in a record.</param>
    /// <param name="rowTerminator">The string used to separate rows in the data.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="fieldTerminator"/> or <paramref name="rowTerminator"/> is null.</exception>
    /// <exception cref="ArgumentException">Thrown if <paramref name="fieldTerminator"/> is the same as <paramref name="rowTerminator"/>.</exception>
    public SetExtractionConfiguration(string fieldTerminator, string rowTerminator)
    {
        VerifyProperties(fieldTerminator, rowTerminator);
        IsICustomObject = false;
    }//ctor main

    /// <summary>
    /// Initializes a new instance of the <see cref="SetExtractionConfiguration{T}"/> class with field and row terminators and a custom object converter.
    /// </summary>
    /// <param name="fieldTerminator">The string used to separate fields in a record.</param>
    /// <param name="rowTerminator">The string used to separate rows in the data.</param>
    /// <param name="_converter">The custom converter used to convert a string literal into an object of type <typeparamref name="T"/>.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="_converter"/> is null.</exception>
    public SetExtractionConfiguration(string fieldTerminator, string rowTerminator, ICustomObjectConverter<T> _converter)
        : this(fieldTerminator, rowTerminator)
    {
        // Check for nulls
        ArgumentNullException.ThrowIfNull(_converter, nameof(_converter));
        Converter = _converter;
    }//ctor 2

    // Methods

    /// <summary>
    /// Verifies that the field and row terminators are not null and not the same.
    /// </summary>
    /// <param name="_fieldTerminator">The field terminator to check.</param>
    /// <param name="_rowTerminator">The row terminator to check.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="_fieldTerminator"/> or <paramref name="_rowTerminator"/> is null.</exception>
    /// <exception cref="ArgumentException">Thrown if <paramref name="_fieldTerminator"/> is the same as <paramref name="_rowTerminator"/>.</exception>
    private void VerifyProperties(string _fieldTerminator, string _rowTerminator)
    {
        // Check if they are not null
        ArgumentNullException.ThrowIfNull(_fieldTerminator, nameof(_fieldTerminator));
        ArgumentNullException.ThrowIfNull(_rowTerminator, nameof(_rowTerminator));

        // Check if they are not the same
        if (_fieldTerminator == _rowTerminator)
            throw new ArgumentException("Terminators cannot be the same.");

        // Assign terminators
        FieldTerminator = _fieldTerminator;
        RowTerminator = _rowTerminator;
    }//VerifyProperties

    /// <summary>
    /// Converts a record string to an object of type <typeparamref name="T"/> using the specified converter.
    /// </summary>
    /// <param name="record">The record string to convert to an object.</param>
    /// <returns>An object of type <typeparamref name="T"/> representing the record.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the <see cref="Converter"/> is null.</exception>
    public T ToObject(string record)
    {
        // Check for nulls
        ArgumentNullException.ThrowIfNull(this.Converter, nameof(this.Converter));

        // Convert
        return this.Converter.ToObject(record, this);
    }//ToObject
}//class
//namespace