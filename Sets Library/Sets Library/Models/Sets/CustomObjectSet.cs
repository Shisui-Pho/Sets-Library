﻿/*
 * File: CustomObjectSet.cs
 * Author: Phiwokwakhe Khathwane
 * Date: 29 March 2025
 * 
 * Description:
 * This file contains the implementation of the CustomObjectSet<T> class, 
 * which represents a custom set capable of holding elements of type T. 
 * The elements in the set must implement both the IComparable<T> and 
 * ICustomObjectConverter<T> interfaces. This set allows for operations 
 * such as checking for the presence of an element and merging with 
 * other sets, although the methods are currently not implemented.
 * 
 * Key Features:
 * - Defines a set for elements that are comparable and convertible from a string.
 * - Provides a placeholder method for checking element presence (Contains).
 * - Includes a method for merging sets (MergeWith) with future implementation.
 */
namespace SetsLibrary;

/// <summary>
/// Represents a custom set that can contain elements of type T, 
/// which must be comparable and convertible from a string representation.
/// </summary>
/// <typeparam name="T">The type of elements in the set, which must implement <see cref="IComparable{T}"/> and <see cref="ICustomObjectConverter{T}"/>.</typeparam>
public class CustomObjectSet<T> : BaseSet<T>
    where T : IComparable<T>, ICustomObjectConverter<T>
{
    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="CustomObjectSet{T}"/> class with the specified extraction configuration.
    /// </summary>
    /// <param name="extractionConfiguration">The configuration to be used for extracting set elements and subsets.</param>
    public CustomObjectSet(SetsConfigurations extractionConfiguration)
        : base(extractionConfiguration)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CustomObjectSet{T}"/> class with the specified string expression and extraction configuration.
    /// </summary>
    /// <param name="expression">The string representation of the set expression.</param>
    /// <param name="config">The configuration to be used for extracting set elements and subsets.</param>
    public CustomObjectSet(string expression, SetsConfigurations config)
        : base(expression, config)
    {
    }
    /// <summary>
    /// Initializes a new instance of the <see cref="CustomObjectSet{T}"/> class by injecting an existing instance of IIndexedSetTree.
    /// This constructor allows for complete control over the set tree, including the expression and extraction settings.
    /// </summary>
    /// <param name="indexedSetTree">An existing instance of an IIndexedSetTree that provides both the set elements 
    /// and configuration for extraction.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="indexedSetTree"/> is null.</exception>
    public CustomObjectSet(IIndexedSetTree<T> indexedSetTree) : base(indexedSetTree)
    {
    }
    #endregion Constructors

    #region Overrides

    /// <summary>
    /// Checks if the current instance is a custom object set.
    /// </summary>
    /// <returns>True if it is a custom object set.</returns>
    protected sealed override SetsConfigurations ModifyConfigurations(SetsConfigurations config)
    {
        //Here we create a new instance of the Extraction settings using the properties we had
        var configNew = new CustomSetsConfigurations<T>(config.FieldTerminator, config.RowTerminator,config.IgnoreEmptySets, config.AutomaticallyAddBrace);
        //Set the method for converting
        configNew.Funct_ToObject = ToObject;
        //Override the base configuration with the new one
        return configNew;
    }

    /// <summary>
    /// Builds and returns a new <see cref="CustomObjectSet{T}"/> based on the provided string representation of the set.
    /// </summary>
    /// <param name="setString">The string representation of the set to be created.</param>
    /// <returns>A new instance of <see cref="CustomObjectSet{T}"/>.</returns>
    public override IStructuredSet<T> BuildNewSet(string setString)
    {
        return new CustomObjectSet<T>(setString, this.ExtractionConfiguration);
    }//BuildNewSet

    /// <summary>
    /// Builds and returns a new, empty <see cref="CustomObjectSet{T}"/>.
    /// </summary>
    /// <returns>A new, empty instance of <see cref="CustomObjectSet{T}"/>.</returns>
    public override IStructuredSet<T> BuildNewSet()
    {
        return new CustomObjectSet<T>(this.ExtractionConfiguration);
    }//BuildNewSet
    /// <summary>
    /// Builds and returns a new set based on the provided indexed set tree wrapper.
    /// </summary>
    /// <param name="tree">The indexed tree of the set</param>
    /// <returns>A new instance of structured set</returns>
    protected internal override IStructuredSet<T> BuildNewSet(IIndexedSetTree<T> tree)
    {
        return new CustomObjectSet<T>(tree);
    }//BuildNewSet

    #endregion Overrides

    /// <summary>
    /// Converts a record string to an object of type <typeparamref name="T"/> using the specified converter.
    /// </summary>
    /// <param name="fields">The record string to convert to an object.</param>
    /// <returns>An object of type <typeparamref name="T"/> representing the record.</returns>
    /// <exception cref="SetsConfigurationException">Thrown if conversion failed.</exception>
    public static T ToObject(string?[] fields)
    {
        try
        {
            return T.ToObject(fields);
        }
        catch
        (Exception e)
        {
            throw new SetsConfigurationException("Unable to complete conversion.", "Check the stack trace.", e);
        }
    }//ToObject
} // class