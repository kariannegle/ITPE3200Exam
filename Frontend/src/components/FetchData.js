// src/components/FetchData.js
import React, { useEffect, useState } from "react"
import axios from "../api/axios"

const FetchData = () => {
  const [data, setData] = useState([])
  const [error, setError] = useState("")

  useEffect(() => {
    const fetchData = async () => {
      try {
        const response = await axios.get("/your-endpoint") // Replace with your endpoint
        setData(response.data)
      } catch (error) {
        setError("An error occurred while fetching data.")
      }
    }

    fetchData()
  }, [])

  return (
    <div>
      <h1>Fetched Data</h1>
      {error && <p style={{ color: "red" }}>{error}</p>}
      <ul>
        {data.map((item) => (
          <li key={item.id}>{item.name}</li> // Adjust based on your data structure
        ))}
      </ul>
    </div>
  )
}

export default FetchData
