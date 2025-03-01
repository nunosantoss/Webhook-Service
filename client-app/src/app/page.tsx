/* eslint-disable @typescript-eslint/no-explicit-any */
"use client";
import { useEffect, useState } from "react";
import { HubConnectionBuilder } from "@microsoft/signalr";
import Card from "@mui/material/Card";
import CardActions from "@mui/material/CardActions";
import CardContent from "@mui/material/CardContent";
import Button from "@mui/material/Button";
import Typography from "@mui/material/Typography";
import axios from "axios";
import { Box } from "@mui/material";
import Image from "next/image";

export default function Home() {
  const [message, setMessage] = useState("");
  const price = 100;

  const hubURL: string = process.env.SOCKET_CONNECTION ?? "";
  const paymentRoute = process.env.PAYMENT_ROUTE ?? "";

  useEffect(() => {
    const connection = new HubConnectionBuilder()
      .withUrl(hubURL, {
        withCredentials: true,
      })
      .withAutomaticReconnect()
      .build();

    connection
      .start()
      .then(() => console.log("Connected to WebSocket"))
      .catch((err) => console.error("Connection failed: ", err));

    connection.on("ReceivePaymentUpdate", (status: any) => {
      console.log("Received Payment Status:", status);
      setMessage(status);
    });

    return () => {
      connection.stop();
    };
  }, []);

  const initTransaction = async () => {
    try {
      await axios.post(paymentRoute, {
        amount: price,
      });
    } catch {
      setMessage("Error initiating payment.");
    }
  };

  return (
    <Box display={"flex"} justifyContent={"center"} alignItems={"center"}>
      <Card sx={{ minWidth: 275 }}>
        <CardContent>
          <Box display={"flex"} justifyContent={"center"} alignItems={"center"}>
            <Image src={"/nike.png"} width={200} height={200} alt="shoe" />
          </Box>
          <Box display={"flex"} justifyContent={"center"} alignItems={"center"}>
            <Typography
              gutterBottom
              sx={{ color: "text.secondary", fontSize: 14 }}
            >
              Nike Air Force 1
            </Typography>
          </Box>
          <Typography variant="caption" fontWeight={600}>
            Payment Status
          </Typography>
          <Typography variant="subtitle2">
            {message || "Waiting for payment updates..."}
          </Typography>
          <Box
            display={"flex"}
            justifyContent={"center"}
            alignItems={"center"}
            pt={2}
          >
            <Typography variant="h5" fontWeight={600}>
              {price}$
            </Typography>
          </Box>
        </CardContent>
        <CardActions>
          <Button
            variant="contained"
            size="small"
            onClick={() => initTransaction()}
          >
            Purchase
          </Button>
        </CardActions>
      </Card>
    </Box>
  );
}
