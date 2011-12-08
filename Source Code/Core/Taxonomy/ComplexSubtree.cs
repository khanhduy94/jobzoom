﻿// This collection of non-binary tree data structures created by Dan Vanderboom.
// Critical Development blog: http://dvanderboom.wordpress.com
// Original Tree<T> blog article: http://dvanderboom.wordpress.com/2008/03/15/treet-implementing-a-non-binary-tree-in-c/

using System;
using System.Text;

namespace JobZoom.Core.Taxonomy
{
    /// <summary>
    /// Represents a hierarchy of objects or data.  ComplexSubtree is an alias for ComplexTree and ComplexTreeNode.
    /// </summary>
    public class ComplexSubtree<T> : ComplexTreeNode<T> where T : ComplexTreeNode<T>
    {
        public ComplexSubtree() { }
    }
}