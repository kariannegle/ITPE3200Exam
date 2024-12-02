import React, { useEffect, useState } from "react"
import { useNavigate } from "react-router-dom"
import axios from "../api/axios"
import userImg from "../assets/image.png"
import CreatePost from "../components/CreatePost"
import Comments from "../components/Comment"

const Posts = () => {
  const [posts, setPosts] = useState([])
  const [error, setError] = useState("")
  const [loading, setLoading] = useState(true)
  const navigate = useNavigate()

  const fetchPosts = async () => {
    setLoading(true)
    setError("")
    try {
      const response = await axios.get("/post")
      if (response.data && response.data.$values) {
        const transformedPosts = response.data.$values.map((post) => ({
          ...post,
          comments: post.comments?.$values || [],
        }))
        transformedPosts.sort(
          (a, b) => new Date(b.createdAt) - new Date(a.createdAt)
        )
        setPosts(transformedPosts)
      } else {
        setError("Unexpected response format")
      }
    } catch (err) {
      setError("An error occurred while fetching posts.")
    } finally {
      setLoading(false)
    }
  }

  const handleDeletePost = async (postId) => {
    try {
      await axios.delete(`/post/${postId}`)
      setPosts(posts.filter((post) => post.id !== postId))
    } catch (error) {
      console.error("Error deleting post:", error)
    }
  }

  const handleEditPost = (postId) => {
    navigate(`/edit/${postId}`)
  }

  useEffect(() => {
    fetchPosts()
  }, [])

  const formatDate = (dateString) => {
    return new Date(dateString).toLocaleDateString(undefined, {
      year: "numeric",
      month: "short",
      day: "numeric",
      hour: "2-digit",
      minute: "2-digit",
    })
  }

  if (loading) return <p>Loading posts...</p>

  return (
    <div className="main-content mt-5">
      <div className="content-inner">
        <CreatePost onPostCreated={fetchPosts} />
        <h1 className="posts-header">LATEST POSTS</h1>
        {error && <div className="alert alert-danger">{error}</div>}
        {posts.map((post) => (
          <div className="post" key={post.id}>
            <div className="left-content">
              {post.imageData ? (
                <div className="imagecontent">
                  <img
                    src={`data:image/jpeg;base64,${post.imageData}`}
                    alt="Post"
                    aria-label="Post image"
                  />
                </div>
              ) : (
                <div className="notecontentwithoutimage">
                  <p aria-label="Post content">{post.content}</p>
                </div>
              )}
            </div>
            <div className="right-content">
              <div
                className="post-details-box"
                aria-label="Post details and comments"
              >
                <div className="post-header">
                  <div className="userImg">
                    <img src={userImg} alt="User profile" />
                  </div>
                  <a
                    className="nav-link text-black"
                    href="#"
                    aria-label="User profile"
                  >
                    <span className="username" aria-label="Username">
                      {post.username}
                    </span>
                  </a>
                  <div className="post-date" aria-label="Post date">
                    {formatDate(post.createdAt)}
                  </div>
                </div>
                <div className="noteContentWithImage">
                  <p>{post.content}</p>
                </div>
                <button
                  className="btn btn-danger"
                  onClick={() => handleDeletePost(post.id)}
                >
                  Delete
                </button>
                <button
                  className="btn btn-primary"
                  onClick={() => handleEditPost(post.id)}
                >
                  Edit
                </button>
              </div>
              {/* Comments Section */}
              <Comments comments={post.comments} postId={post.id} />
            </div>
          </div>
        ))}
      </div>
    </div>
  )
}

export default Posts
