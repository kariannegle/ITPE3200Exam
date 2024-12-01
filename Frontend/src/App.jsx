import React, { useState } from "react"
import { BrowserRouter as Router, Route, Routes } from "react-router-dom"
import "./App.css"
import Navbar from "./components/Navbar"
import Posts from "./components/Posts"
import Register from "./components/Register"
import Login from "./components/Login"
import Settings from "./components/Settings"

const App = () => {
  const [isSignedIn, setIsSignedIn] = useState(false)

  // Mock function to handle login/logout
  const handleLogin = (user) => {
    setIsSignedIn(true)
  }

  const handleLogout = () => {
    setIsSignedIn(false)
  }

  return (
    <Router>
      <div>
        <Navbar isSignedIn={isSignedIn} handleLogout={handleLogout} />

        <Routes>
          <Route path="/register" element={<Register />} />
          <Route path="/login" element={<Login />} />
          <Route path="/settings" element={<Settings />} />
          <Route path="/" element={<Posts />} />
          {/* Add other routes here */}
        </Routes>
      </div>
    </Router>
  )
}

export default App
