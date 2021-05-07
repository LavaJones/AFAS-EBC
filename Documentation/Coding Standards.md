# var keyword
Here are the guidelines for the usage of the var keyword.
+ Use var when you have to; when you are using anonymous types. 
+ Use var when the type of the declaration is obvious from the initializer, especially if it is an object creation. This eliminates redundancy. 
+ Consider using var if the code emphasizes the semantic "business purpose" of the variable and downplays the "mechanical" details of its storage. 
+ Use explicit types if doing so is necessary for the code to be correctly understood and maintained. 
+ Use descriptive variable names regardless of whether you use "var". Variable names should represent the semantics of the variable, not details of its storage; "decimalRate" is bad; "interestRate" is good.