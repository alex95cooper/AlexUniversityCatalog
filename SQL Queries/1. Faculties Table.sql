CREATE DATABASE University

USE University

CREATE TABLE Faculties
(
ID INT IDENTITY (1,1) ,
Name  VARCHAR(50) NOT NULL UNIQUE,
Description TEXT,
PRIMARY KEY (ID)
)

INSERT INTO Faculties
(Name, Description)
VALUES 
('Aircraft Construction', 'The Faculty of Aircraft Engineering is one of the founding faculties of the University. Our faculty successfully trains highly qualified specialists capable of solving the entire complex of tasks in the field of design, computer-aided design, production, repair and maintenance, testing and certification, as well as problems of environmental safety and transport maintenance of modern aviation equipment.'),
('Aviation Engines', 'The Faculty of Aviation Engines is one of the basic faculties of the National Aerospace University named after M. E. Zhukovsky "KHAI". It was founded together with the Kharkiv Aviation Institute in 1930. During its existence, the faculty trained thousands of highly qualified specialists who made a brilliant career at the most famous enterprises of Ukraine, the EU, the USA and other countries. '),
('Aircraft Aontrol Systems', 'The activity of the "Aircraft Control Systems" faculty has the main goal: the training of personnel in a wide range of specialties that meet the modern requirements of the specialized labor market. As of 2020, the faculty includes 5 graduating departments and 4 branch departments at specialized enterprises. The training of bachelors, specialists, masters, postgraduates and doctoral students is carried out by a teaching staff of 95 teachers, and approximately 1,000 students, including foreigners with English-language education, study at the facultys 14 educational programs.'),
('Rocket-Space Engineering', 'Today, the Faculty of Rocket and Space Engineering of KAI is: the only faculty of Ukraine and one of the few in the world, which trains competitive specialists capable of creating state-of-the-art rocket and space complexes; teaching professionally oriented disciplines in English; state-of-the-art powerful computer centers and networks with free Internet access via Wi-Fi in the university and dormitories'),
('Radio Electronics', 'Faculty of radio electronics, computer systems and information communications. The faculty was founded in 1959. At this time, about 100 teachers and research staff train students in the design and production of aviation radio-electronic systems and networks, etc. Graduates are in high demand not only in Ukraine, but all over the world.'),
('Software engineering', 'The Faculty of Software Engineering and Business was founded in 1991. Good basic training allows graduates to successfully adapt to the modern economic environment and achieve rapid career growth. The high level of preparation of graduates of the faculty is ensured by in-depth   study of basic managerial, economic, humanitarian disciplines, the content of which is regularly updated.'),
('Humanities and Law', 'The humanitarian and legal faculty was founded in 1999 and unites five departments: philosophy and social sciences, law, applied linguistics, psychology, physical education, sports and health. The faculty successfully trains highly qualified specialists in the humanitarian profile, who are in demand on the modern labor market. Our graduates hold positions in leading institutions both in Ukraine and abroad.'),
('International Communications', 'The Faculty of International Communications and Training of Foreign Citizens aims to train highly qualified specialists in the specialty "Information, library and archival affairs".')

SELECT*FROM Faculties
