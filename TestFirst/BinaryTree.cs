using System;
using System.Runtime.Serialization;

namespace TestFirst {
    public class BinaryTree {
        private BinaryNode _root;

        public void Add(int info) {
            if (_root != null)
            {
                _root.AddNode(new BinaryNode(info));
            }
            else
            {
                _root = new BinaryNode(info);
            }
        }

        private BinaryNode Search(int info) {
            try
            {
                var res = _root.Search(info);
                return res;
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine("You need add at least 1 node");
                throw;
            }
        }

        public void Delete(int info) {
            var node = _root.Search(info);
            node.DeleteNode();
        }

        public void PrintTree() {
            Console.WriteLine("------------------------------------");
            _root.PrintInfo();
            Console.WriteLine("------------------------------------");
        }
    }

    internal class BinaryTreeException : Exception {
        public BinaryTreeException() {
        }

        public BinaryTreeException(string message) : base(message) {
        }

        public BinaryTreeException(string message, Exception innerException) : base(message, innerException) {
        }

        protected BinaryTreeException(SerializationInfo info, StreamingContext context) : base(info, context) {
        }
    }

    internal class BinaryNode {
        private BinaryNode _leftNode;
        private BinaryNode _rightNode;
        private BinaryNode _parent;
        public int Info { get; set; }

        public BinaryNode(int info) {
            Info = info;
        }

        public void AddNode(BinaryNode node) {
            if (node.Info < Info)
            {
                CheckInternalNode(ref _leftNode, node);
            }
            else if (node.Info > Info)
            {
                CheckInternalNode(ref _rightNode, node);
            }
        }

        public BinaryNode Search(int info) {
            if (Info == info)
                return this;
            var node = info > Info ? _rightNode.Search(info) : _leftNode.Search(info);
            if (node == null)
                throw new BinaryTreeException("There is no node with that value");
            return node;
        }

        private void CheckInternalNode(ref BinaryNode internalNode, BinaryNode nodeToAdd) {
            if (internalNode == null)
            {
                nodeToAdd._parent = this;
                internalNode = nodeToAdd;
            }
            else
            {
                internalNode.AddNode(nodeToAdd);
            }
        }

        public void DeleteNode() {
            if (_leftNode == null && _rightNode == null)
            {
                ReplaceParentNode(null);
            }
            else if (_leftNode != null ^ _rightNode != null)
            {
                ReplaceParentNode(_leftNode ?? _rightNode);
            }
            else
            {
                var node = _rightNode.FindMin();
                Info = node.Info;
                node.DeleteNode();
            }
        }

        private void ReplaceParentNode(in BinaryNode newNode) {
            if (newNode != null)
                newNode._parent = _parent;
            if (_parent == null) return;
            if (_parent._leftNode == this)
                _parent._leftNode = newNode;
            else
                _parent._rightNode = newNode;
        }

        private BinaryNode FindMin() {
            var node = this;
            while (node._leftNode != null)
            {
                node = node._leftNode;
            }

            return node;
        }

        public override string ToString() {
            return $"I:{Info}|P:{_parent?.Info}|LC:{_leftNode?.Info}|RC:{_rightNode?.Info}|";
        }

        public void PrintInfo() {
            Console.WriteLine(ToString());
            _leftNode?.PrintInfo();
            _rightNode?.PrintInfo();
        }
    }
}