# EF Core many-to-many
Testing Entity Framework Core many to many update in ASP.Net MVC Core.

The scenario is:
- Schools have many Students.
- Students have many Courses.
- Courses have many Students.

So we have a **one-to-many** relasionship between *School* and *Students* and a **many-to-many** relationship between *Students* and *Courses*.

We should be able to create and edit a school with students and courses in the same view.
