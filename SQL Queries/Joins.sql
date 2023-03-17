USE University

SELECT FirstName, LastName, Age, Experience, Name AS Subjects
FROM Teachers
INNER JOIN Subjects
ON Teachers.SubjID = Subjects.ID

SELECT FirstName, LastName, Age, Year, Faculties.Name AS Faculty, STRING_AGG(Subjects.Name, ',') AS Subjects FROM Students_Subjects
    INNER JOIN Students ON Students.ID = Students_Subjects.StudID
    INNER JOIN Faculties ON Students.FacID = Faculties.ID
    LEFT JOIN Subjects ON Students_Subjects.SubjID = Subjects.ID    
	GROUP BY Students.ID, FirstName, LastName, Age, Year, Faculties.Name 
	ORDER BY Students.ID ASC OFFSET 0 ROWS FETCH NEXT 5 ROWS ONLY


     
SELECT StudID, STRING_AGG (SubjID, ', ')  AS Subjects FROM Students_Subjects
GROUP BY StudID

SELECT FirstName, LastName, Age, Year, Faculties.Name AS Faculty, STRING_AGG(Subjects.Name, ',') AS Subjects FROM Students_Subjects
INNER JOIN Students ON Students.ID = Students_Subjects.StudID
INNER JOIN Faculties ON Students.FacID = Faculties.ID
LEFT JOIN Subjects ON Students_Subjects.SubjID = Subjects.ID
GROUP BY Students.ID, FirstName, LastName, Age, Year, Faculties.Name  
ORDER BY Students.ID ASC  OFFSET 0 ROWS FETCH NEXT 5 ROWS ONLY

