﻿/*
 * File: SetsOperations.cs
 * Author: Phiwokwakhe Khathwane
 * Date: 24 December 2024
 * 
 * Description:
 * Defines the SetsOperations static class, which provides methods for performing 
 * set operations on IStructuredSet<T> objects. The operations include intersection, 
 * union, and complement, with support for custom set implementations that implement 
 * IStructuredSet<T>.
 * 
 * Key Features:
 * - IntersectWith: Computes the intersection of two sets using elements and subsets.
 * - UnionWith: Merges two sets into one, retaining all unique elements.
 * - Complement: Computes the complement of a set with respect to a universal set.
 * - Ensures null checks and provides useful error handling for invalid set operations.
 * 
 * The class makes use of the IComparable<T> constraint for type safety and performance in set operations.
 */

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace SetsLibrary.SetOperations;

/// <summary>
/// Provides various set operations such as intersection, union, and complement
/// for <typeparam > <see cref="IStructuredSet{T}"/> objects, where T is a type that implements <see cref="IComparable{T}"/>.</typeparam>
/// </summary>
public static class SetsOperations
{
    /// <summary>
    /// Computes the intersection of two sets, returning a new set containing
    /// only the elements and subsets common to both sets.
    /// </summary>
    /// <typeparam name="TElement">The type of elements in the sets, which must implement <see cref="IComparable{TElement}"/>.</typeparam>
    /// <param name="A">The first set to intersect.</param>
    /// <param name="B">The second set to intersect.</param>
    /// <returns>A new set containing the intersection of <paramref name="A"/> and <paramref name="B"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown if either <paramref name="A"/> or <paramref name="B"/> is null.</exception>
    /// <exception cref="SetsOperationException">Thrown if an error occurs during the intersection operation.</exception>
    public static IStructuredSet<TElement> IntersectWith<TElement>([NotNull] this IStructuredSet<TElement> A, IStructuredSet<TElement> B)
        where TElement : IComparable<TElement>
    {

        #region Mathematical Approach
        ////Check for nulls
        //ArgumentNullException.ThrowIfNull(A, nameof(A));
        //ArgumentNullException.ThrowIfNull(B, nameof(B));

        //#region Explanation example
        //// A = 1 2 3 4 5 6 7 8 9
        //// B = 4 7 9 0
        //// ( B - A ) : 0             <--- C
        //// ( A - B ) : 1 2 3 5 6 8   <--- D
        //// ( C U D ) : 0 1 2 3 5 6 8 <--- E
        //// ( A - E ) : 4 7 9         <--- This is ( A intersect B )

        ////So in general we have that given sets A and B such that the universal set = A U B, then :
        //// - A intersect B = A - ( B - A ) U ( A - B)
        //#endregion Explanation example

        ////Here we work on the assume that the union of A and B is the universal set
        ////- C = B - A
        //var C = B.Without(A);
        ////- D = A - B
        //var D = A.Without(B);
        ////- E = C U D
        //var E = C.UnionWith(D);
        //// A intersect B = A - E
        //var result = A.Without(E);

        ////Return the intersection
        //return result;



        //Option B
        //Since we're assuming that the Union of A and B is equal to the universal set
        //Then this can also be simplified to be:
        //- A intersect B = A - (Symmetric difference of A and B)
        //return A.Without(A.SymmetricDifference(B));
        #endregion Mathematical Approach
        // Check for null sets using ArgumentNullException.ThrowIfNull
        ArgumentNullException.ThrowIfNull(A, nameof(A));
        ArgumentNullException.ThrowIfNull(B, nameof(B));

        try
        {
            // Initialize a new empty set to store the result
            var newSet = A.BuildNewSet();

            // Enumerate and intersect the root elements of both sets
            var A_rootsElements = A.EnumerateRootElements().ToHashSet();
            var B_rootsElements = B.EnumerateRootElements().ToHashSet();
            var intersectionRoots = A_rootsElements.Intersect(B_rootsElements);

            foreach (var root in intersectionRoots)
                newSet.AddElement(root);

            // Enumerate and intersect the subsets of both sets
            var A_subsets = A.EnumerateSubsets().ToHashSet();
            var B_subsets = B.EnumerateSubsets().ToHashSet();
            var intersectionSubsets = A_subsets.Intersect(B_subsets);

            foreach (var subset in intersectionSubsets)
                newSet.AddElement(subset);

            return newSet;
        }
        catch (Exception ex)
        {
            // Log the error and rethrow as a SetsOperationException with the message
            throw new SetsOperationException(
                "An error occurred while performing the intersection operation.",
                "Error during the intersection of two sets.",
                ex
            );
        }
    }//IntersectWith

    /// <summary>
    /// Computes the union of two sets, returning a new set containing all unique elements
    /// from both sets.
    /// </summary>
    /// <typeparam name="TElement">The type of elements in the sets, which must implement <see cref="IComparable{TElement}"/>.</typeparam>
    /// <param name="current">The current set to union.</param>
    /// <param name="setB">The set to union with <paramref name="current"/>.</param>
    /// <returns>A new set containing the union of <paramref name="current"/> and <paramref name="setB"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown if either <paramref name="current"/> or <paramref name="setB"/> is null.</exception>
    /// <exception cref="SetsOperationException">Thrown if an error occurs during the union operation.</exception>
    public static IStructuredSet<TElement> UnionWith<TElement>(this IStructuredSet<TElement> current, IStructuredSet<TElement> setB)
        where TElement : IComparable<TElement>
    {
        // Use ArgumentNullException.ThrowIfNull for null checks
        ArgumentNullException.ThrowIfNull(current, nameof(current));
        ArgumentNullException.ThrowIfNull(setB, nameof(setB));

        try
        {
            // Just merge the sets
            return current.MergeWith(setB);
        }
        catch (Exception ex)
        {
            // Log the error and rethrow as a SetsOperationException with the message
            throw new SetsOperationException(
                "An error occurred while performing the union operation.",
                "Error during the union of two sets.",
                ex
            );
        }
    }//UnionWith

    /// <summary>
    /// Computes the complement of a set with respect to a universal set.
    /// </summary>
    /// <typeparam name="TElement">The type of elements in the sets, which must implement <see cref="IComparable{TElement}"/>.</typeparam>
    /// <param name="A">The set to compute the complement for.</param>
    /// <param name="universalSet">The universal set used to compute the complement.</param>
    /// <returns>A new set containing the complement of <paramref name="A"/> with respect to <paramref name="universalSet"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown if either <paramref name="A"/> or <paramref name="universalSet"/> is null.</exception>
    /// <exception cref="SetsOperationException">Thrown if an error occurs during the complement operation.</exception>
    public static IStructuredSet<TElement> Complement<TElement>(this IStructuredSet<TElement> A, IStructuredSet<TElement> universalSet)
        where TElement : IComparable<TElement>
    {
        // Use ArgumentNullException.ThrowIfNull for null checks
        ArgumentNullException.ThrowIfNull(A, nameof(A));
        ArgumentNullException.ThrowIfNull(universalSet, nameof(universalSet));


        // The universal set must have a cardinality greater than or equal to the set A
        if (A.Cardinality > universalSet.Cardinality)
        {
            throw new SetsOperationException(
                $"Cardinality of the set {nameof(A)} must be less or equal to the cardinality of the {nameof(universalSet)}",
                "The cardinality of set A exceeds that of the universal set."
            );
        }

        try
        {
            // If the cardinality of set A is 0, the complement is the entire universal set
            if (A.Cardinality == 0)
            {
                return universalSet;
            }

            // Return the complement of A with respect to the universal set using the "Without" method
            return A.Without(universalSet);
        }
        catch (Exception ex)
        {
            // Log the error and rethrow as a SetsOperationException with the message
            throw new SetsOperationException(
                "An error occurred while performing the complement operation.",
                "Error during the complement of the set.",
                ex
            );
        }
    }//Complement

    /// <summary>
    /// Computes the difference between two sets, returning a new set containing elements
    /// that are in <paramref name="A"/> but not in <paramref name="B"/>.
    /// </summary>
    /// <typeparam name="TElement">The type of elements in the sets, which must implement <see cref="IComparable{TElement}"/>.</typeparam>
    /// <param name="A">The first set.</param>
    /// <param name="B">The second set to subtract from <paramref name="A"/>.</param>
    /// <returns>A new set containing the difference of <paramref name="A"/> and <paramref name="B"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown if either <paramref name="A"/> or <paramref name="B"/> is null.</exception>
    /// <exception cref="SetsOperationException">Thrown if an error occurs during the difference operation.</exception>
    public static IStructuredSet<TElement> Difference<TElement>(this IStructuredSet<TElement> A, IStructuredSet<TElement> B)
        where TElement : IComparable<TElement>
    {
        // Check for nulls
        ArgumentNullException.ThrowIfNull(A, nameof(A));
        ArgumentNullException.ThrowIfNull(B, nameof(B));

        try
        {
            // Just use the without method
            return A.Without(B);
        }
        catch (Exception ex)
        {
            // Log the error and rethrow as a SetsOperationException with the message
            throw new SetsOperationException(
                "An error occurred while performing the difference operation.",
                "Error during the difference of two sets.",
                ex
            );
        }
    }//Difference

    /// <summary>
    /// Computes the symmetric difference between two sets, returning a new set
    /// containing elements that are in either <paramref name="A"/> or <paramref name="B"/>,
    /// but not in both.
    /// </summary>
    /// <typeparam name="TElement">The type of elements in the sets, which must implement <see cref="IComparable{TElement}"/>.</typeparam>
    /// <param name="A">The first set.</param>
    /// <param name="B">The second set.</param>
    /// <returns>A new set containing the symmetric difference of <paramref name="A"/> and <paramref name="B"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown if either <paramref name="A"/> or <paramref name="B"/> is null.</exception>
    /// <exception cref="SetsOperationException">Thrown if an error occurs during the symmetric difference operation.</exception>
    public static IStructuredSet<TElement> SymmetricDifference<TElement>(this IStructuredSet<TElement> A, IStructuredSet<TElement> B)
        where TElement : IComparable<TElement>
    {
        //Check for nulls
        ArgumentNullException.ThrowIfNull(A, nameof(A));
        ArgumentNullException.ThrowIfNull(B, nameof(B));

        //This can be done by computing (A - B) ∪ (B - A).
        try
        {
            return (A.Difference(B)).UnionWith(B.Difference(A));
        }
        catch (Exception ex)
        {
            // Log the error and rethrow as a SetsOperationException with the message
            throw new SetsOperationException(
                "An error occurred while performing the symmetric difference operation.",
                "Error during the symmetric difference of two sets.",
                ex
            );
        }
    }//SymmetricDifference

    /// <summary>
    /// Computes the Cartesian product of two sets, returning a new set containing all
    /// possible ordered pairs of elements from <paramref name="A"/> and <paramref name="B"/>.
    /// </summary>
    /// <typeparam name="TElement">The type of elements in the sets, which must implement <see cref="IComparable{TElement}"/>.</typeparam>
    /// <param name="A">The first set.</param>
    /// <param name="B">The second set.</param>
    /// <returns>A new set containing the Cartesian product of <paramref name="A"/> and <paramref name="B"/>.</returns>
    /// <exception cref="NotImplementedException">Thrown because this operation is not yet implemented.</exception>
    public static IStructuredSet<(TElement, TElement)> CartesianProduct<TElement>(this IStructuredSet<TElement> A, IStructuredSet<TElement> B)
        where TElement : IComparable<TElement>
    {
        throw new NotImplementedException("A very complex procedure");
    }//CartesianProduct

    /// <summary>
    /// Determines whether two sets are disjoint (i.e., they have no elements in common).
    /// </summary>
    /// <typeparam name="TElement">The type of elements in the sets, which must implement <see cref="IComparable{TElement}"/>.</typeparam>
    /// <param name="A">The first set.</param>
    /// <param name="B">The second set.</param>
    /// <returns><c>true</c> if the sets are disjoint, <c>false</c> otherwise.</returns>
    /// <exception cref="ArgumentNullException">Thrown if either <paramref name="A"/> or <paramref name="B"/> is null.</exception>
    /// <exception cref="SetsOperationException">Thrown if an error occurs during the disjoint operation.</exception>
    public static bool IsDisjoint<TElement>(this IStructuredSet<TElement> A, IStructuredSet<TElement> B)
       where TElement : IComparable<TElement>
    {
        //Check for nulls
        ArgumentNullException.ThrowIfNull(A, nameof(A));
        ArgumentNullException.ThrowIfNull(B, nameof(B));

        try
        {
            //If they are both empty
            if (A.Cardinality == 0 && B.Cardinality == 0)
                return false;

            // C = B - A
            // if C = B ==> A and B are disjoint
            if (A.Cardinality >= B.Cardinality)
                return B.Without(A).SetStructuresEqual(B);

            //Then B is larger than A
            return A.Without(B).SetStructuresEqual(A);
        }
        catch (Exception ex)
        {
            // Log the error and rethrow as a SetsOperationException with the message
            throw new SetsOperationException(
                "An error occurred while performing the a check for disjoint operation.",
                "Check the stack trace.",
                ex
            );
        }

    }//IsDisjoint

    /// <summary>
    /// Determines whether the structures of two sets are equal (i.e., they contain the same elements).
    /// </summary>
    /// <typeparam name="TElement">The type of elements in the sets, which must implement <see cref="IComparable{TElement}"/>.</typeparam>
    /// <param name="A">The first set.</param>
    /// <param name="B">The second set.</param>
    /// <returns><c>true</c> if the sets contain the same elements, <c>false</c> otherwise.</returns>
    /// <exception cref="ArgumentNullException">Thrown if either <paramref name="A"/> or <paramref name="B"/> is null.</exception>
    /// <exception cref="SetsOperationException">Thrown if an error occurs during the equality check.</exception>
    public static bool SetStructuresEqual<TElement>(this IStructuredSet<TElement> A, IStructuredSet<TElement> B)
        where TElement : IComparable<TElement>
    {
        //Check for nulls
        ArgumentNullException.ThrowIfNull(A, nameof(A));
        ArgumentNullException.ThrowIfNull(B, nameof(B));

        try
        {
            //Check for empty sets
            if (A.Cardinality == 0 && B.Cardinality == 0)
                return true;

            //Build the set strings and compare them
            string elementString1 = A.BuildStringRepresentation();
            string elementString2 = B.BuildStringRepresentation();

            return elementString1.Equals(elementString2);
        }
        catch (Exception ex)
        {
            // Log the error and rethrow as a SetsOperationException with the message
            throw new SetsOperationException(
                "An error occurred while checking if two sets are equal.",
                "Check the stack trace.",
                ex
            );
        }
    }//SetStructuresEqual
}//class