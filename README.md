Prime Rule engine

System requirements :

OS : Windows 7 or above.

External Dependencies(dlls) :
None.

Steps to use

1.Upload file,
2.Add rule
3.Save and apply rule




Read the tooltip for better understanding

Parameter name -  Name of the parameter. Ex : value

Parameter value -  Param value must be type of Integer/Datetime/String. If type is 				string, then only type HIGH or LOW (case sensitive)

Operator - Conditions supported as of now for Integer and Datetime : 
         '< or less than', '> or greater than', '<= or less than or equal to', '>= 		or greater than or equal to'. For string : 'contains', 'startswith', 		'endswith'. '== or equal to' and '!= or not equal to' is common for all types.

Key parameter -  KeyParameter name Ex : signal

Key parameter value - KeyParameter value should be alpha numeric.

SaveRule - Validate and save rule without applying to file.

ApplyRule - Validate, apply basic rule (Basic rule about signal,value and value_type 			given in the assignment) , apply rule mentioned and store the rule.

All file contents which are violating the rule will be shown in the ‘Datagrid’

Solution approach




1.MVVM pattern applied
2.Rule is created using RuleElement class.
3.Each file content is a object of DataUnit
4.Each DataUnit is validated against RuleElement using Singleton RuleValidator class.
5.Each rule will be stored in RuleBase.txt using model class RuleManager
6.Loosely coupled code using MVVM pattern.

Code complexity and performance

Code complexity - O(n)
Performance to execute the above shown rule - <18 ms , cpu usage 0.5%
Bottlenecks - condition evaluation.

Improvements

1.Retrieving and applying stored rules
2.Applying AND OR condition between rules
3.Better file stream like sqlite
4.Edit and save violated content in UI.
5.Look for any performance loop hole.
6.More condition statements.















