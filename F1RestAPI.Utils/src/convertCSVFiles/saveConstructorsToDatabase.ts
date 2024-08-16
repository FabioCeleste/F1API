import { Constructor } from "../types";
const fs = require("fs");
const csv = require("csv-parser");
const { PrismaClient } = require("@prisma/client");
const path = require("path");

const prisma = new PrismaClient();

const getItems = async () => {
    const constructorsCSVFile = path.join(
        __dirname,
        "dataset",
        "constructors.csv"
    );

    fs.createReadStream(constructorsCSVFile)
        .pipe(csv())
        .on("data", async (constructor: Constructor) => {
            await prisma.constructors.create({
                data: {
                    ConstructorId: parseInt(constructor.constructorId),
                    ConstructorRef: constructor.constructorRef,
                    Name: constructor.name,
                    Nationality: constructor.nationality,
                    Url: constructor.url,
                },
            });
        })
        .on("end", async () => {
            console.log("Constructors CSV file successfully processed");
        });
};

getItems();
