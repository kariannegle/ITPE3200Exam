// src/App.js
import React from "react"
import "./App.css"
import Logo from "./assets/logo.png"
import Posts from "./components/Posts"

const App = () => {
  return (
    <div>
      <a class="navbar-brand" href="/" aria-label="Home">
        <img
          src={Logo}
          alt="SnapNoteLogo"
          width="100"
          height="40"
          class="d-inline-block align-text-top"
        />
      </a>
      <Posts />
    </div>
  )
}

export default App
