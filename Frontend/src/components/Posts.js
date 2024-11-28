// src/components/Posts.js
import React, { useEffect, useState } from "react"
import axios from "../api/axios"
import userImg from "../assets/image.png"

const Posts = () => {
  const [posts, setPosts] = useState([])
  const [error, setError] = useState("")

  useEffect(() => {
    const fetchPosts = async () => {
      try {
        const response = await axios.get("/post")
        if (response.data && response.data.$values) {
          const transformedPosts = response.data.$values.map((post) => ({
            ...post,
            comments: post.comments.$values,
          }))
          setPosts(transformedPosts)
        } else {
          setError("Unexpected response format")
        }
      } catch (error) {
        setError("An error occurred while fetching posts.")
      }
    }

    fetchPosts()
  }, [])

  return (
    <div className="main-content mt-5">
      <div className="content-inner">
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
                    href="/"
                    aria-label="User profile"
                  >
                    <span className="username" aria-label="Username">
                      {post.username}
                    </span>
                  </a>
                  <div className="post-date" aria-label="Post date">
                    {new Date(post.createdAt).toLocaleDateString()}
                  </div>
                </div>
                {post.imageData ? (
                  <div className="noteContentWithImage">
                    <p>{post.content}</p>
                  </div>
                ) : (
                  <div className=""></div>
                )}
              </div>
              {/* Comments section */}
              <h5
                className="comments-header"
                aria-label="Comments section header"
              >
                Comments
              </h5>
              <div className="discussions">
                <div
                  className="comments-scrollable"
                  aria-label="Scrollable comments list"
                >
                  {post.comments.map((comment) => (
                    <div key={comment.id}>
                      <div className="discussion mb-3"> </div>
                      <div className="d-flex justify-content-between align-items-center">
                        <div className="d-flex align-items-center">
                          {/* User Info */}
                          <div className="userImg d-flex align-items-center">
                            <a href="/" className="nav-link text-black">
                              <img
                                src={userImg}
                                alt="Avatar"
                                style={{ width: "25px", height: "25px" }}
                              />
                              <span className="username">
                                {comment.username}
                              </span>
                            </a>
                          </div>
                          <div className="comment-date">
                            <small>
                              {new Date(comment.createdAt).toLocaleDateString()}
                            </small>
                          </div>
                        </div>
                      </div>
                      <div className="comment">
                        <p>{comment.content}</p>
                      </div>
                    </div>
                  ))}
                </div>
              </div>
            </div>
          </div>
        ))}
      </div>
    </div>
  )
}

export default Posts
