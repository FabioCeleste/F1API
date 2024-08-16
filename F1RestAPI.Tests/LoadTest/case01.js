import http from "k6/http";
import { sleep } from "k6";
import { check } from "k6";

export let options = {
    stages: [
        { duration: "10s", target: 2 },
        { duration: "20s", target: 15 },
        { duration: "6m", target: 1300 },
    ],
};

export default function () {
    const url = "https://f1restapi.com.br/api/drivers/all";
    const params = {
        headers: {
            "Content-Type": "application/json",
            "User-Agent": "Agente do Caos - 2023",
        },
    };

    let res = http.get(url, params);

    check(res, {
        "status is 200": (r) => {
            console.log(r.status);

            return [200].includes(r.status);
        },
    });

    sleep(Math.random() * (0.03 - 0.001) + 0.001);
}
