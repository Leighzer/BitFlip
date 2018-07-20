# Bit Flip

A language made for fun by Leighton Covington

# Introduction

Bit Flip is a programming language geared towards manipulating data at the binary level with a minimal instruction set.

Bit Flip is very similar to assembly language of today's machines. It is essentially assembly code for a virtual machine where all bits are addressable, and memory is no more complex than a simple tape of binary digits.

Bit Flip is primarily inspired by the language Brain Fuck and other minimalist languages that are very simple in nature, and still turing complete. Bit Flip is also inspired by today's assembly languages for modern machines.

Bit Flip isn't supposed to be a human practical programming language. Bit Flip isn't meant to be a practical language with today's machines that are based on at least bytes.

The goal of the language is to supply a simple to understand instruction set that is capable of universal computation all while working at the binary level. At the end of the day computers just change and modify zeroes and ones, and the spirit of Bit Flip is to provide a language that would allow the user to work at the most fundamental level of information processing. Flipping Bits. The goal of Bit Flip is to capture the most basic way to process information, by Flipping Bits.

The vision behind Bit Flip is the following. What if we wanted to create a bare bones machine that was turing complete. What if we didn't need to settle for being limited to addressing bytes and could address any single bit. What if we could modify/toggle/set/read singular addressable bits EXTREMELY EXTREMELY fast. It is as simple as flipping a switch. What if this could be faster than today's machines that process bytes with standard programs. What if the programmer using this machine didn't need all the syntactic sugar that typical humans need to code. Enter Bit Flip.

Please note that this is very much just speculation. There is definitely a reason modern computers have developed the way they have. If really any of the above what ifs are incorrect than the Bit Flip machine in practice wouldn't be very great. This language is most definitely a turing tar pit. With this note made, let's move on.

The target user of this language would be machines. Possible uses of Bit Flip is we can create machines that are able to create Bit Flip programs that accomplish the same behaviors written by humans, all while hopefully being smaller in size, faster, and obfuscated. Intelligent enough AI wouldn't need all the syntactical sugar humans need. They would just need the absolute raw instructions to make programs. They could utilize languages with just one instructions for all of their programming needs. This is language is definitely not claiming to be the absolute minimal language in existance, but an interesting idea how minimal the data processing the language is. Just flipping bits.


# Structure of Bit Flip

The structure of bit flip is the following.

|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|

                   ^      Head
                   U = 0  Bucket
                   F = 0  Flag

There is a tape of arbitrary length that holds binary digits. This is our working memory where we will perform data operations.

There is a tape head, that points to a singular digit on our tape. It is commonly referred to as just head. This is what will be used to indicate what index of the tape we are currently working with. A head points to a singular index on a tape.

There is a bucket, this is temporary storage like a binary clip board, that allows us to temporarily remember a value we copied from the tape, and to paste this saved value else where. The bucket is also indirectly a part of the conditional branching functionality of the language.

There is a flag, this is a temporary storage that is used by conditional instruction(s) to branch. The flag is set/modified ONLY by the test instruction.

# The Instruction Set:

Tape/Bucket/Flag instructions (Data operations):

toggle - flip the bit that the head is currently pointing at.

set <arg> - set the bit the head is currently pointing at to arg.
  
copy - copy the bit the head is pointing at to the bucket.

write - write the bucket's binary value to the memory location the head is pointing at.

test - set flag value to bucket && value @ head.


Moving the tape head:

right - move the head one index to the right

left - move the head one index to the left

Flow of execution/Jumping/Looping:
jump <label> - jump flow of execution to label location in program
  
cjump <label> - if flag is true, jump flow of execution to label loaction in program, else fall through to next instruction
  
exit - close the program

# Language Lexemes and Semantic Structures
The language's statements follow these structures

< instruction >;

< instruction > < arg >;

< label >:


# What is currently being worked on:

This is the initial language specification. Right now there is a very bare bones compiler in the works. The current plan of attack is to write a source to source compiler in a high level language that can take a Bit Flip program and translate it into an equivalent c++ program. Once there the compiler will invoke the c++ compiler (g++) and create the Bit Flip executable.


# Down the Road: Future Functionality For Bit Flip

I/O:
Basic I/O:
With the current version there is no I/O specificed, which is a large part of computation. The plan is to allow a Bit Flip program to load/copy any file into its tape, allow the program to modify the tape, and arbitrarily write the bits over the old file, or into a new file. 

Dynamic Tape Creation:
An idea in the works of I/O is to allow the dynamic instantiation of tapes during program execution, or to allow multiple tapes the head can jump around on.
IE declaring a new tape, then using an I/O instruction to load a file into that tape.This would require new instructions to load files from a path into a tape. This would also require some new instructions that would move the current head from tape to tape. This functionality could make it much easier to create Bit Flip programs that write other Bit Flip programs.


Enhanced Heads: Bridges
Bridges are an idea of having one object similar to a head, that has not just one head/pointer, but multiple pointers/heads. With this added flexibility there could be added instructions that could add more power. One such instruction could be && all the values the bridge has pointers to, and save that value in the bucket.


Multiple Heads:
One possible performance increase would be to introduce multiple separate heads working in the same program, working on the same tape. One could treat each head as a thread, and parallelize a Bit Flip program. One problem would be overlapping heads modifying data, which head would get the last say, how would we avoid race conditions between heads. There would need to be rules in place that would lead to consistent program behavior with multiple heads.


More Conditional Instructions/Variations of vanilla instructions:
These instructions are instructions I thought about including, but decided not to in favor of keeping the language simple. The added c at the start of the instruction stands for conditional. These instructions I imagine could help drastically cut down how many head moves would be required in a program, at least I think.

Tape/Bucket/Flag instructions (Data operations):
ctoggle - if flag is true, flip the bit that the head is currently pointing at. else, do nothing
cset <arg> - if flag is true, set the bit the head is currently pointing at to arg. else, do nothing
ccopy - if flag is true, copy the bit the head is pointing at to the bucket. else, do nothing
cwrite - if flag is true, write the bucket's binary value to the memory location the head is pointing at. else, do nothing
ctest - if flag is true, set flag value to bucket && value @ head.
  
Moving the tape head:
cright - if flag is true, move the head one index to the right. else, do nothing
cleft - if flag is true, move the head one index to the left. else, do nothing
