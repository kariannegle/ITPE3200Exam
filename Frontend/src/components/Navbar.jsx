import React from "react";
import { Link } from "react-router-dom";
import Logo from "../assets/logo.png";

const Navbar = ({ isSignedIn, username, handleLogout }) => {
  return (
    <nav className="navbar navbar-expand-md navbar-light fixed-top">
      <div className="container-fluid">
        {/* Sidebar Toggle Button */}
        <button
          className="btn btn-outline-secondary me-2 d-md-none"
          type="button"
          data-bs-toggle="collapse"
          data-bs-target="#sidebar"
          aria-controls="sidebar"
          aria-expanded="false"
          aria-label="Toggle sidebar"
        >
          <i className="bi bi-list"></i>
        </button>

        {/* Navbar Brand */}
        <Link className="navbar-brand" to="/" aria-label="Home">
          <img
            src={Logo}
            alt="SnapNoteLogo"
            width="100"
            height="40"
            className="d-inline-block align-text-top"
          />
        </Link>

        {/* Navbar Content (Right-aligned) */}
        <div className="collapse navbar-collapse" id="navbarContent">
          <ul className="navbar-nav ms-auto">
            {isSignedIn ? (
              <>
                {/* Settings Icon */}
                <li className="nav-item">
                  <Link className="nav-link" to="/settings" aria-label="Settings">
                    <i className="bi bi-gear" alt="SettingIcon" style={{ fontSize: "1.5rem" }}></i>
                  </Link>
                </li>
                {/* Profile Icon with Dropdown */}
                <li className="nav-item dropdown">
                  <a
                    className="nav-link dropdown-toggle"
                    href="#"
                    id="navbarDropdown"
                    role="button"
                    data-bs-toggle="dropdown"
                    aria-expanded="false"
                  >
                    <i className="bi bi-person-circle" style={{ fontSize: "1.5rem" }} aria-label="Profile Icon"></i>
                  </a>
                  <ul className="dropdown-menu dropdown-menu-end" aria-labelledby="navbarDropdown">
                    <li>
                      <Link className="dropdown-item" to="/settings">Settings</Link>
                    </li>
                    <li>
                      <button className="dropdown-item" onClick={handleLogout}>Logout</button>
                    </li>
                  </ul>
                </li>
              </>
            ) : (
              <>
                {/* Login and Register Links for larger screens */}
                <li className="nav-item d-none d-md-block">
                  <Link className="nav-link link-underline link-underline-opacity-0 text-dark" to="/login">Login</Link>
                </li>
                <li className="nav-item d-none d-md-block">
                  <Link className="nav-link link-underline-opacity-0 text-dark" to="/register">Register</Link>
                </li>
              </>
            )}
          </ul>
        </div>
      </div>
    </nav>
  );
};

export default Navbar;