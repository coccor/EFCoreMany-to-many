# EFCoreMany-to-many
Testing Entity Framework Core many to many update in ASP.Net MVC Core.

The scenario is:

School has many Students
Students has many Courses
Courses has many Students

So we have a onet-to-many relasionshib between School and Students and a many-to-many relationship between Students and Courses.

We should be able to create and edit a school with students and courses in the same view.
