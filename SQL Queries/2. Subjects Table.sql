USE University

DROP TABLE Subjects

CREATE TABLE Subjects
(
ID INT IDENTITY (1,1) ,
FacID INT NOT NULL,
Name  VARCHAR(50) NOT NULL UNIQUE,
Description TEXT,
PRIMARY KEY (ID),
FOREIGN KEY (FacID) REFERENCES Faculties (ID)
)

INSERT INTO Subjects
(FacID, Name, Description)
VALUES 
(1,'Aircraft Construction','Flight is one of the marvels of the modern age. The complex aerospace engineering that makes it possible is nothing short of astounding. From flight-critical systems to connectivity and entertainment — flight control processors and collision avoidance, to sensors, lighting or communications.'),
(1,'Strength of materials','The field of strength of materials (also called mechanics of materials) typically refers to various methods of calculating the stresses and strains in structural members, such as beams, columns, and shafts. '),
(2,'Aircraft Engine Technology','The Department of Aircraft Engine Production Technology is an educational unit of the Faculty of Engine Engineering, focused on the training of highly qualified personnel in the field of mechanical processing technology, assembly and testing; specialists in 3D modeling and design of operations on modern machines with numerical software control, etc.'),
(2,'Standardization','Standardization is the process of developing, promoting and possibly mandating standards-based and compatible technologies and processes within a given industry.'),
(3,'Electrical Engineering','Electrical engineers design, develop, test, and supervise the manufacture of electrical equipment, such as electric motors, radar and navigation systems, communications systems, or power generation equipment.'),
(3,'Informatics','Informatics is the study of computational systems. According to the ACM Europe Council and Informatics Europe, informatics is synonymous with computer science and computing as a profession, in which the central notion is transformation of information.'),
(4,'Mathematics','Mathematics is an area of knowledge that includes the topics of numbers, formulas and related structures, shapes and the spaces in which they are contained, and quantities and their changes. These topics are represented in modern mathematics with the major subdisciplines of number theory, algebra, geometry, and analysis, respectively.'),
(4,'Sketch Geometry','Geometrical drawing focuses on the use of geometric shapes to create designs, patterns, and more complex artwork. These drawings could be as simple as a basic doodle or as complex as a sketch for a geometrical painting. You can also focus on simple 2D shapes or work on 3D forms. You don’t need much to get started – just some pencils and paper.'),
(5,'Physics','Physics is the branch of science that deals with the structure of matter and how the fundamental constituents of the universe interact. It studies objects ranging from the very small using quantum mechanics to the entire universe using general relativity.'),
(5,'Radio Electronics','Studying of Radio-electronic Devices, System and Complexes'),
(6,'Economy','An economy is an area of the production, distribution and trade, as well as consumption of goods and services. In general, it is defined as a social domain that emphasize the practices, discourses, and material expressions associated with the production, use, and management of scarce resources.'),
(6,'Organization of production','The organization of production is a process whereby production factors such as labour, capital and land are coordinated within and across organizations to produce goods and services. This includes an organizations managerial practices and routines, and its cooperative ties to other organizations.'),
(7,'Philosophy','Philosophy (from Greek: philosophia, love of wisdom) is the systematized study of general and fundamental questions, such as those about existence, reason, knowledge, values, mind, and language.'),
(7,'English','English is a West Germanic language in the Indo-European language family, with its earliest forms spoken by the inhabitants of early medieval England')

SELECT*FROM Subjects