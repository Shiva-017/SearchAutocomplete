using System;
using System.Collections.Generic;

class Node {
    public Dictionary<char, Node> children;
    public bool isEnd;  // marks end of a complete word
    
    public Node() {
        children = new Dictionary<char, Node>();
        isEnd = false; // default: not a word boundary
    }
}

class Trie {
    private Node root;
    
    public Trie() {
        root = new Node();
    }

    // Insert a word into the trie
    public void Insert(string word) {
        Node temp = root;
        foreach (char ch in word) {
            if (!temp.children.ContainsKey(ch)) {
                temp.children[ch] = new Node();
            }
            temp = temp.children[ch];
        }
        temp.isEnd = true;
    }

    // Return true if word exists exactly in the trie
    public bool Search(string word) {
        Node temp = root;
        foreach (char ch in word) {
            if (!temp.children.ContainsKey(ch)) {
                return false;
            }
            temp = temp.children[ch];
        }
        return temp.isEnd;
    }

    // Delete a word from the trie; returns true if node was removed
    public bool Delete(string word) {
        return deleteHelper(word, 0, root);  // start traversal from root
    }

    private bool deleteHelper(string word, int index, Node node) {
        if (index == word.Length) {
            if (!node.isEnd) {
                return false;
            }
            node.isEnd = false;
            return node.children.Count == 0;  // safe to prune if leaf
        }

        char ch = word[index];
        if (!node.children.ContainsKey(ch)) {
            return false;
        }

        bool shouldDelete = deleteHelper(word, index + 1, node.children[ch]);
        if (shouldDelete) {
            node.children.Remove(ch);
            return node.children.Count == 0 && !node.isEnd;
        }
        return false;
    }

    // Return all words stored in the trie
    public List<string> ListAllWords() {
        List<string> words = new List<string>();
        ListAllWordsHelper("", root, words);
        return words;
    }

    private void ListAllWordsHelper(string word, Node node, List<string> words) {
        if (node.isEnd) {
            words.Add(word);
        }
        foreach (var child in node.children) {
            ListAllWordsHelper(word + child.Key, child.Value, words);
        }
    }

    // Return all words that start with the given prefix
    public List<string> ListAllWordsWithPrefix(string prefix) {
        List<string> words = new List<string>();
        Node temp = root;
        foreach (char ch in prefix) {
            if (!temp.children.ContainsKey(ch)) {
                return words;
            }
            temp = temp.children[ch];
        }
        Dfs(temp, prefix, words);
        return words;
    }

    // DFS helper to collect all words reachable from node
    private void Dfs(Node node, string path, List<string> words) {
        if (node.isEnd) {
            words.Add(path);
        }
        foreach (var child in node.children) {
            Dfs(child.Value, path + child.Key, words);
        }
    }

    // static void Main(string[] args) {
    //     Trie trie = new Trie();
    //     trie.Insert("apple");
    //     trie.Insert("app");
    //     trie.Insert("apples");
    //     trie.Insert("applet");
    //     trie.Insert("applesauce");
        
    //     Console.WriteLine(trie.Search("apple")); // true
    //     Console.WriteLine(string.Join(", ", trie.ListAllWords())); 
    //     Console.WriteLine(string.Join(", ", trie.ListAllWordsWithPrefix("apple"))); 
    //     Console.WriteLine(trie.Delete("apple")); // true
    //     Console.WriteLine(trie.Search("apple")); // false
    //     Console.WriteLine(string.Join(", ", trie.ListAllWords())); 
    // }
}