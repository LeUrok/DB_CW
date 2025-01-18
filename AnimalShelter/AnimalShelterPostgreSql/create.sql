CREATE TABLE MedicalCard(
    Id SERIAL PRIMARY KEY,
    AnimalId INT,
    DateOfLastVisit TIMESTAMP NOT NULL,
    Diagnoses TEXT NOT NULL,
    Therapy TEXT NOT NULL
);

CREATE TABLE AnimalType(
    Id SERIAL PRIMARY KEY,
    Name TEXT NOT NULL
);

INSERT INTO AnimalType (Name) VALUES ('Кошка'), ('Собака');

CREATE TABLE AnimalBreed(
    Id SERIAL PRIMARY KEY,
    Name TEXT NOT NULL
);
INSERT INTO AnimalBreed (Name) VALUES ('Нет');

CREATE TABLE Animal(
    Id SERIAL PRIMARY KEY,
    Name TEXT NOT NULL,
    AnimalTypeId INT REFERENCES AnimalType(Id) ON DELETE CASCADE,
    AnimalBreedId INT REFERENCES AnimalBreed(Id) ON DELETE CASCADE,
    Gender INT NOT NULL,
    Age INT NOT NULL,
    Weight DOUBLE PRECISION NOT NULL,
    Description TEXT NOT NULL,
    MedicalCardId INT REFERENCES MedicalCard(Id) ON DELETE SET NULL
);
ALTER TABLE MedicalCard
ADD FOREIGN KEY (AnimalId) REFERENCES Animal(Id) ON DELETE CASCADE;
CREATE TABLE Guardian(
    Id SERIAL PRIMARY KEY,
    UserId INT,
    StartDate DATE NOT NULL,
    EndDate DATE 
);
CREATE TABLE Animal_Guardian(
    AnimalId INT REFERENCES Animal(Id) ON DELETE CASCADE,
    GuardianId INT REFERENCES Guardian(Id) ON DELETE CASCADE,
    PRIMARY KEY (AnimalId, GuardianId)
);
CREATE TABLE Employee(
    Id SERIAL PRIMARY KEY,
    UserId INT,
    Job TEXT NOT NULL
);
CREATE TABLE ShelterUser(
    Id SERIAL PRIMARY KEY,
    IdEmployee INT REFERENCES Employee(Id),
    GuardianId INT REFERENCES Guardian(Id),
    Name TEXT NOT NULL,
    Phone TEXT NOT NULL,
    Mail TEXT NOT NULL
);
ALTER TABLE Employee
ADD FOREIGN KEY (UserId) REFERENCES ShelterUser(Id) ON DELETE CASCADE;
ALTER TABLE Guardian
ADD FOREIGN KEY (UserId) REFERENCES ShelterUser(Id) ON DELETE CASCADE;

DO $$
DECLARE
    user_id bigint;
BEGIN
    INSERT INTO ShelterUser (Name, Phone, Mail) 
    VALUES ('Admin', '', '')
    RETURNING Id INTO user_id;

    INSERT INTO Employee (UserId, Job) 
    VALUES (user_id, 'administrator');

    UPDATE ShelterUser
    SET IdEmployee = user_id
    WHERE Id = user_id;
END $$;

CREATE ROLE anonim;
GRANT SELECT ON TABLE ShelterUser, Employee to anonim; 
ALTER ROLE anonim WITH LOGIN;

CREATE ROLE administrator WITH LOGIN PASSWORD 'admin_pswrd';
GRANT INSERT, UPDATE, DELETE, SELECT ON ALL TABLES IN SCHEMA public TO administrator;
GRANT USAGE, SELECT ON SEQUENCE medicalcard_id_seq,
                                animaltype_id_seq,
                                animalbreed_id_seq,
                                animal_id_seq,
                                guardian_id_seq,
                                employee_id_seq,
                                shelteruser_id_seq TO administrator;


CREATE ROLE assistant WITH LOGIN PASSWORD 'assistant_pswrd';
GRANT UPDATE, DELETE, INSERT, SELECT ON Animal, AnimalType, AnimalBreed, 
                                        Guardian, Shelteruser, Animal_Guardian TO assistant;
GRANT SELECT ON Employee, MedicalCard TO assistant;
GRANT INSERT ON MedicalCard TO assistant;
GRANT USAGE, SELECT ON SEQUENCE animal_id_seq, 
                                medicalcard_id_seq,
                                guardian_id_seq, 
                                animaltype_id_seq,
                                shelteruser_id_seq,
                                animalbreed_id_seq TO assistant;


CREATE ROLE vet WITH LOGIN PASSWORD 'vet_pswrd';
GRANT SELECT ON Animal, Employee, AnimalType, AnimalBreed, shelteruser,
                Animal_Guardian, Guardian TO vet;
GRANT UPDATE, DELETE, INSERT, SELECT ON MedicalCard TO vet;

CREATE OR REPLACE FUNCTION GetAnimalsByBreed(breed_id INT)
RETURNS TABLE (
    animal_id INT,
    name TEXT,
    animal_type_id INT,
    animal_breed_id INT,
    gender INT,
    age INT,
    weight DOUBLE PRECISION,
    description TEXT,
    medical_card_id INT
) AS $$
BEGIN
    RETURN QUERY
    SELECT
        a.Id AS animal_id,
        a.Name AS name,
        a.AnimalTypeId AS animal_type_id,
        a.AnimalBreedId AS animal_breed_id,
        a.Gender AS gender,
        a.Age AS age,
        a.Weight AS weight,
        a.Description AS description,
        a.MedicalCardId AS medical_card_id
    FROM Animal AS a
    WHERE a.AnimalBreedId = breed_id;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION GetAnimalsByType(type_id INT)
RETURNS TABLE (
    animal_id INT,
    name TEXT,
    animal_type_id INT,
    animal_breed_id INT,
    gender INT,
    age INT,
    weight DOUBLE PRECISION,
    description TEXT,
    medical_card_id INT
) AS $$
BEGIN
    RETURN QUERY
    SELECT
        a.Id AS animal_id,
        a.Name AS name,
        a.AnimalTypeId AS animal_type_id,
        a.AnimalBreedId AS animal_breed_id,
        a.Gender AS gender,
        a.Age AS age,
        a.Weight AS weight,
        a.Description AS description,
        a.MedicalCardId AS medical_card_id
    FROM Animal AS a
    WHERE a.AnimalTypeId = type_id;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION GetAnimalsByTypeAndBreed(type_id INT, breed_id INT)
RETURNS TABLE (
    animal_id INT,
    name TEXT,
    animal_type_id INT,
    animal_breed_id INT,
    gender INT,
    age INT,
    weight DOUBLE PRECISION,
    description TEXT,
    medical_card_id INT
) AS $$
BEGIN
    RETURN QUERY
    SELECT
        a.Id AS animal_id,
        a.Name AS name,
        a.AnimalTypeId AS animal_type_id,
        a.AnimalBreedId AS animal_breed_id,
        a.Gender AS gender,
        a.Age AS age,
        a.Weight AS weight,
        a.Description AS description,
        a.MedicalCardId AS medical_card_id
    FROM Animal AS a
    WHERE a.AnimalTypeId = type_id and a.AnimalBreedId = breed_id;
END;
$$ LANGUAGE plpgsql;