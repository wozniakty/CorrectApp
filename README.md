# CorrectApp

## Description

This is a little simple app to practice implementing autocorrect (without reading guides on how to do it).  Seemed like a fun way to practice DSA stuff.
Also, gives me the opportunity to check out more Blazor stuff, .NET 5, and C# 9.0

## Demo
See it live (assuming I remember to deploy the latest) https://correctapp.azurewebsites.net/autocorrect

## Algorithms

### Autocomplete After Error
After the first letter that doesn't match any word, it suggests all the possible words matching the initial set.  More like auto-complete than auto-correct, and only captures the first error.

### Closest Matching Triplets
So I took some time to think about my experience with Elasticsearch, which uses tokenization of strings into substrings for a variety of comparisons. This gave me the idea to take a common pattern
(triplets) and split words into sets of triplets, and split the input into sets of triplets, and find as many matches as possible.

I therefore didn't spend time getting it to work on words smaller than 3, and it also struggles with words of very few letters as it is likely to contain less matches :|
For example taht has no triplets in common with that, despite being obviously similar.

### Find Matching Triplets
Simply returns ALL results with *any* matches, ordered from most number of matches to least

### Find close Matching Triplets
After the above gave way too many results, I realized I probably needed some other way to see how similar a word was, which led me to https://en.wikipedia.org/wiki/Levenshtein_distance
Using this, I found matching triplets, then found the levenshtein distance between the results and the input, and return any with a distance lower than 3.

### Find closest matching triplets.
After the above, I then thought it might be best to only return the set of matches with the *lowest* distance, giving us a more focused set of results while also allowing the distance to go up higher
if there's only less similar matches.