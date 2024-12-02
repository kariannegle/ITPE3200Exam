import React from 'react';
import { Navbar, Nav, Container, Button } from 'react-bootstrap';
import { Link } from 'react-router-dom';
import { useAuth } from '../context/AuthContext'; // Import the useAuth hook
import '../App.css';

const CustomNavbar = () => {
  const { isAuthenticated, logout } = useAuth(); // Use authentication status and logout function

  return (
    <Navbar expand="md" bg="light" variant="light" fixed="top" className="navbar">
      <Container fluid>
        <Button
          variant="outline-secondary"
          className="me-2 d-md-none"
          onClick={() => console.log('Sidebar toggle clicked')}
        >
          <i className="fas fa-bars"></i>
        </Button>

        <Navbar.Brand as={Link} to="/">SnapNote</Navbar.Brand>
        <Navbar.Toggle aria-controls="basic-navbar-nav" />
        <Navbar.Collapse id="basic-navbar-nav">
          <Nav className="ms-auto">
            {isAuthenticated ? (
              <>
                <Nav.Link as={Link} to="/">Home</Nav.Link>
                <Nav.Link as={Link} to="/posts">Posts</Nav.Link>
                <Nav.Link as={Link} to="/logout" onClick={logout}>Logout</Nav.Link>
              </>
            ) : (
              <>
                <Nav.Link as={Link} to="/login">Login</Nav.Link>
                <Nav.Link as={Link} to="/register">Register</Nav.Link>
              </>
            )}
          </Nav>
        </Navbar.Collapse>
      </Container>
    </Navbar>
  );
};

export default CustomNavbar;
